using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Calabonga.HorseOnTheGrid.Core;
using Microsoft.Extensions.Logging;

namespace Calabonga.HorseOnTheGrid.Mobile.ViewModels;

/// <summary>
/// Модель представления игры: связывает ядро (<see cref="Game"/>) с экраном.
/// Сама правил не знает — только переводит состояние партии в состояние клеток и текстов.
/// </summary>
public partial class GameViewModel : ObservableObject
{
    private const string BestResultKey = "best_result";

    private readonly Game _game;
    private readonly IPreferences _preferences;
    private readonly ILogger<GameViewModel> _logger;

    [ObservableProperty]
    private string _progressText = string.Empty;

    [ObservableProperty]
    private int _bestResult;

    [ObservableProperty]
    private string _statusText = string.Empty;

    [ObservableProperty]
    private bool _isGameOver;

    public GameViewModel(Game game, IPreferences preferences, ILogger<GameViewModel> logger)
    {
        _game = game;
        _preferences = preferences;
        _logger = logger;

        var size = game.Board.Size;
        var cells = new List<CellViewModel>(size * size);
        for (var row = 0; row < size; row++)
        {
            for (var column = 0; column < size; column++)
            {
                cells.Add(new CellViewModel(new Position(column, row), OnCellSelected));
            }
        }

        Cells = cells;
        BestResult = preferences.Get(BestResultKey, 0);

        _game.Changed += (_, _) => Refresh();
        Refresh();
    }

    /// <summary>Все клетки поля построчно: сначала верхняя строка слева направо.</summary>
    public IReadOnlyList<CellViewModel> Cells { get; }

    [RelayCommand]
    private void NewGame()
    {
        _logger.LogInformation("Новая партия");
        _game.Start();
    }

    [RelayCommand(CanExecute = nameof(CanUndo))]
    private void Undo()
    {
        _game.Undo();
        _logger.LogInformation("Ход отменён, посещено клеток: {Count}", _game.MoveCount);
    }

    private bool CanUndo() => _game.CanUndo;

    private void OnCellSelected(CellViewModel cell)
    {
        if (_game.TryMove(cell.Position))
        {
            _logger.LogInformation("Ход {Number} на клетку {Position}", _game.MoveCount, cell.Position);
        }
        else
        {
            _logger.LogDebug("Ход на клетку {Position} не по правилам", cell.Position);
        }
    }

    /// <summary>Переносит состояние партии на экран. Вызывается после каждого изменения партии.</summary>
    private void Refresh()
    {
        var board = _game.Board;
        var available = board.GetAvailableMoves().ToHashSet();

        foreach (var cell in Cells)
        {
            cell.Order = board.GetOrder(cell.Position);
            cell.State = cell.Position == board.Current ? CellState.Current
                : available.Contains(cell.Position) ? CellState.Available
                : cell.Order > 0 ? CellState.Visited
                : CellState.Empty;
        }

        ProgressText = $"{_game.MoveCount} из {board.CellCount}";

        if (_game.MoveCount > BestResult)
        {
            BestResult = _game.MoveCount;
            _preferences.Set(BestResultKey, BestResult);
        }

        IsGameOver = _game.State != GameState.InProgress;
        StatusText = _game.State switch
        {
            GameState.Won => "Победа! Конь обошёл все клетки поля.",
            GameState.Stuck => "Тупик: ходов больше нет. Отмените ход или начните заново.",
            _ => "Нажмите на подсвеченную клетку."
        };

        UndoCommand.NotifyCanExecuteChanged();
    }
}
