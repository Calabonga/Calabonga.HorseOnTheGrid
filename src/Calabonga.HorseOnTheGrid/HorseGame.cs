using Calabonga.HorseOntTheGrid.Assets;
using System.Windows.Controls;

namespace Calabonga.HorseOntTheGrid;
public class HorseGame
{
    private readonly LinkedList<Position> _positions = new();

    public HorseGame(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Grid = new CellType[rows, columns];

        AddHorse();
        IsCurrentGameOver = IsGameOver();
    }

    public int Rows { get; }

    public int Columns { get; }

    public CellType[,] Grid { get; }

    public int Score { get; set; }

    public bool IsCurrentGameOver { get; set; }

    private void AddHorse()
    {
        Grid[0, 0] = CellType.Active;
        _positions.AddFirst(new Position(0, 0));
        Score = 1;
    }

    public void MoveHorse(int x, int y, TextBlock textBlock)
    {
        var availableMoves = FindAvailableMoves();
        var canMove = availableMoves.Contains(new Position(x, y));
        if (!canMove)
        {
            IsCurrentGameOver = IsGameOver();
            return;
        }

        RemoveHighlightedCells();

        var newPosition = new Position(x, y);
        Grid[x, y] = CellType.Active;
        var activePosition = _positions.AddLast(newPosition);
        var previous = activePosition.Previous!.Value;
        Grid[previous.Row, previous.Column] = CellType.Filled;
        Score++;
        textBlock.Text = Score.ToString();

        IsCurrentGameOver = IsGameOver();

    }

    private void RemoveHighlightedCells()
    {
        foreach (var position in GetMarkedAsAvailable())
        {
            Grid[position.Row, position.Column] = CellType.Empty;
        }
    }

    private IEnumerable<Position> GetMarkedAsAvailable()
    {
        for (var x = 0; x < Rows; x++)
        {
            for (var y = 0; y < Columns; y++)
            {
                if (Grid[x, y] == CellType.Available)
                {
                    yield return new Position(x, y);
                }
            }
        }
    }

    private bool IsGameOver()
    {
        var availableMoves = FindAvailableMoves();
        if (availableMoves.Any())
        {
            // Add highlighted cell
            AddAvailableMoves(availableMoves);
            return false;
        }

        return true;
    }

    private void AddAvailableMoves(List<Position> positions)
    {
        foreach (var position in positions)
        {
            Grid[position.Row, position.Column] = CellType.Available;
        }
    }

    private List<Position> FindAvailableMoves()
    {
        var activePosition = GetActivePosition();
        var availablePositions = new List<Position>();
        var directions = Direction.Moves();

        foreach (var direction in directions)
        {
            var emptyPositions = GetEmptyPositions();
            var position = activePosition.CheckDirection(direction, emptyPositions);
            if (position == null)
            {
                continue;
            }

            availablePositions.Add(position);
        }

        return availablePositions;
    }

    private IEnumerable<Position> GetEmptyPositions()
    {
        for (var x = 0; x < Rows; x++)
        {
            for (var y = 0; y < Columns; y++)
            {
                if (Grid[x, y] == CellType.Empty || Grid[x, y] == CellType.Available)
                {
                    yield return new Position(x, y);
                }
            }
        }
    }

    private Position GetActivePosition()
    {
        for (var x = 0; x < Rows; x++)
        {
            for (var y = 0; y < Columns; y++)
            {
                if (Grid[x, y] == CellType.Active)
                {
                    return new Position(x, y);
                }
            }
        }

        throw new InvalidOperationException("This is never happen");
    }
}
