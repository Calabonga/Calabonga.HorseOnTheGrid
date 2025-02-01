using Calabonga.HorseOnTheGridMvvm.Game.Entities;
using Calabonga.HorseOnTheGridMvvm.Game.Extensions;
using Calabonga.HorseOnTheGridMvvm.Game.Helpers;
using Calabonga.HorseOnTheGridMvvm.UserControls;

namespace Calabonga.HorseOnTheGridMvvm.Game;

public class HorseGame
{
    private GridCell? _last;

    public HorseGame(int rows, int columns)
    {
        PrepareCells(rows, columns);
        AddHorse();
    }

    public bool GameOver { get; private set; }

    private void AddHorse()
    {
        GridCells.First(x => x is { Row: 0, Column: 0 }).CellType = CellType.Active;
        AddAvailableMoves(GridCells.FindDirectionMoves());
    }

    private void AddAvailableMoves(IEnumerable<GridCell> availableMoves)
    {
        foreach (var cell in availableMoves)
        {
            GridCells
                .First(x => x.Row == cell.Row && x.Column == cell.Column)
                .CellType = CellType.Available;
        }
    }

    public IList<GridCell> GridCells { get; private set; } = [];

    public int Score { get; private set; } = 1;

    private void PrepareCells(int rows, int columns)
    {
        for (var x = 0; x < rows; x++)
        {
            for (var y = 0; y < columns; y++)
            {
                var cell = new GridCell
                {
                    ImageSource = ImageLoader.Empty,
                    Title = x == 0 && y == 0 ? "1" : "",
                    Row = x,
                    Column = y
                };
                GridCells.Add(cell);
            }
        }
    }

    public void MoveHorse(GridCell cell)
    {
        _last = GridCells.GetActiveCell();

        var availableMoves = GridCells.FindDirectionMoves().ToList();

        if (!availableMoves.Contains(cell))
        {
            return;
        }

        Score++;

        // remove highlighted
        availableMoves.ForEach(x => x.CellType = CellType.Empty);

        if (_last is null)
        {
            GridCells.First(x => x is { Row: 0, Column: 0 }).CellType = CellType.Filled;
        }
        else
        {
            GridCells.First(x => x.Row == _last.Row && x.Column == _last.Column).CellType = CellType.Filled;
        }

        cell.Title = Score.ToString();
        cell.CellType = CellType.Active;
        _last = cell;

        var moves = GridCells.FindDirectionMoves().ToList();
        if (!moves.Any())
        {
            GameOver = true;
            return;
        }

        AddAvailableMoves(moves);
    }
}
