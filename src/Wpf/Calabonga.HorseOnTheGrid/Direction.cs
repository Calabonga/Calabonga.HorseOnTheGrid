namespace Calabonga.HorseOntTheGrid;

public record Direction(int RowOffset, int ColumnOffset)
{
    private static readonly Direction LeftUp = new(2, -1);
    private static readonly Direction LeftDown = new(2, 1);

    private static readonly Direction DownLeft = new(-1, 2);
    private static readonly Direction DownRight = new(1, 2);

    private static readonly Direction RightDown = new(-2, 1);
    private static readonly Direction RightUp = new(-2, -1);

    private static readonly Direction UpLeft = new(-1, -2);
    private static readonly Direction UpRight = new(1, -2);

    public static IEnumerable<Direction> Moves()
    {
        yield return LeftUp;
        yield return LeftDown;
        yield return DownLeft;
        yield return DownRight;
        yield return RightDown;
        yield return RightUp;
        yield return UpLeft;
        yield return UpRight;
    }
}
