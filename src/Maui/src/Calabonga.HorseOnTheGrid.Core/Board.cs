namespace Calabonga.HorseOnTheGrid.Core;

/// <summary>
/// Игровое поле: хранит порядковый номер хода для каждой клетки (0 — клетка не посещена)
/// и текущую позицию коня. Изменять поле может только код ядра (команды хода).
/// </summary>
public sealed class Board
{
    public const int DefaultSize = 10;

    private readonly int[,] _order;

    public Board(int size = DefaultSize)
    {
        if (size < 5)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Размер поля должен быть не меньше 5.");
        }

        Size = size;
        _order = new int[size, size];
    }

    /// <summary>Длина стороны поля в клетках.</summary>
    public int Size { get; }

    /// <summary>Общее число клеток.</summary>
    public int CellCount => Size * Size;

    /// <summary>Сколько клеток уже посещено (включая стартовую).</summary>
    public int VisitedCount { get; private set; }

    /// <summary>Где сейчас стоит конь; null — партия ещё не начата.</summary>
    public Position? Current { get; private set; }

    public bool Contains(Position position) =>
        position.Column >= 0 && position.Column < Size &&
        position.Row >= 0 && position.Row < Size;

    public bool IsVisited(Position position) => GetOrder(position) > 0;

    /// <summary>Номер хода, на котором конь посетил клетку; 0 — не посещал.</summary>
    public int GetOrder(Position position)
    {
        EnsureInside(position);
        return _order[position.Column, position.Row];
    }

    /// <summary>Клетки, куда конь может пойти из текущей позиции.</summary>
    public IReadOnlyList<Position> GetAvailableMoves() =>
        Current is { } current ? GetAvailableMovesFrom(current) : [];

    /// <summary>Непосещённые клетки, достижимые ходом коня из <paramref name="from"/>.</summary>
    public IReadOnlyList<Position> GetAvailableMovesFrom(Position from) =>
        KnightMoves.From(from, Size).Where(target => !IsVisited(target)).ToList();

    internal void Visit(Position position)
    {
        EnsureInside(position);
        if (IsVisited(position))
        {
            throw new InvalidOperationException($"Клетка {position} уже посещена.");
        }

        VisitedCount++;
        _order[position.Column, position.Row] = VisitedCount;
        Current = position;
    }

    internal void Unvisit(Position position, Position? previous)
    {
        EnsureInside(position);
        if (_order[position.Column, position.Row] != VisitedCount)
        {
            throw new InvalidOperationException($"Отменить можно только последний ход, а клетка {position} им не является.");
        }

        _order[position.Column, position.Row] = 0;
        VisitedCount--;
        Current = previous;
    }

    internal void Clear()
    {
        Array.Clear(_order);
        VisitedCount = 0;
        Current = null;
    }

    private void EnsureInside(Position position)
    {
        if (!Contains(position))
        {
            throw new ArgumentOutOfRangeException(nameof(position), position, "Клетка находится за пределами поля.");
        }
    }
}
