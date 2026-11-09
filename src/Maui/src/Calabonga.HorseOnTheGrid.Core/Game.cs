namespace Calabonga.HorseOnTheGrid.Core;

/// <summary>
/// Партия «Конь на доске»: поле, история ходов и правила.
/// Интерфейс работает только с этим классом и подписывается на событие <see cref="Changed"/>.
/// </summary>
public sealed class Game
{
    /// <summary>Стартовая клетка — левый верхний угол (для игрока это x:1, y:1).</summary>
    public static readonly Position StartPosition = new(0, 0);

    private readonly CommandHistory _history = new();

    public Game()
        : this(new Board())
    {
    }

    public Game(Board board)
    {
        Board = board;
        Start();
    }

    public Board Board { get; }

    public GameState State { get; private set; }

    /// <summary>Число посещённых клеток, включая стартовую.</summary>
    public int MoveCount => Board.VisitedCount;

    /// <summary>Стартовую клетку отменить нельзя, поэтому отмена доступна только после первого хода.</summary>
    public bool CanUndo => _history.CanUndo;

    /// <summary>Возникает после любого изменения партии: новый старт, ход, отмена.</summary>
    public event EventHandler? Changed;

    /// <summary>Начинает новую партию: очищает поле и ставит коня на стартовую клетку.</summary>
    public void Start()
    {
        Board.Clear();
        _history.Clear();
        Board.Visit(StartPosition);
        UpdateState();
    }

    public bool CanMoveTo(Position target) =>
        State == GameState.InProgress &&
        Board.Current is { } current &&
        Board.Contains(target) &&
        !Board.IsVisited(target) &&
        KnightMoves.IsKnightMove(current, target);

    /// <summary>Пытается сделать ход. Возвращает false, если ход не по правилам.</summary>
    public bool TryMove(Position target)
    {
        if (!CanMoveTo(target))
        {
            return false;
        }

        _history.Execute(new MoveCommand(Board, target));
        UpdateState();
        return true;
    }

    /// <summary>Отменяет последний ход. Работает и после тупика, и после победы.</summary>
    public bool Undo()
    {
        if (!_history.CanUndo)
        {
            return false;
        }

        _history.Undo();
        UpdateState();
        return true;
    }

    private void UpdateState()
    {
        if (Board.VisitedCount == Board.CellCount)
        {
            State = GameState.Won;
        }
        else if (Board.GetAvailableMoves().Count == 0)
        {
            State = GameState.Stuck;
        }
        else
        {
            State = GameState.InProgress;
        }

        Changed?.Invoke(this, EventArgs.Empty);
    }
}
