namespace Calabonga.HorseOntTheGrid;

public record Position(int Row, int Column)
{
    public Position? CheckDirection(Direction direction, IEnumerable<Position> emptyPositions)
    {
        var row = Row + direction.RowOffset;
        var column = Column + direction.ColumnOffset;

        return emptyPositions.FirstOrDefault(x => x.Row == row && x.Column == column);
    }
}
