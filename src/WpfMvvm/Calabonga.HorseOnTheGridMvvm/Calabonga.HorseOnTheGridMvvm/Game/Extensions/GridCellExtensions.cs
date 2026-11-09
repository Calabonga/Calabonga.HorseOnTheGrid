using Calabonga.HorseOnTheGridMvvm.Game.Entities;
using Calabonga.HorseOnTheGridMvvm.UserControls;

namespace Calabonga.HorseOnTheGridMvvm.Game.Extensions;

public static class GridCellExtensions
{
    public static IEnumerable<GridCell> FindDirectionMoves(this IList<GridCell> source)
    {
        var directions = Direction.Moves();
        var emptyPosition = source.EmptyPositions().ToList();
        var activePosition = source.GetActiveCell();

        foreach (var direction in directions)
        {
            var position = activePosition.CheckDirections(direction, emptyPosition);
            if (position == null)
            {
                continue;
            }

            yield return position;
        }
    }

    internal static GridCell GetActiveCell(this IList<GridCell> source)
    {
        return source.First(x => x.CellType is CellType.Active);
    }

    private static IEnumerable<GridCell> EmptyPositions(this IList<GridCell> source)
    {
        return source.Where(x => x.CellType is CellType.Empty or CellType.Available);
    }

    private static GridCell? CheckDirections(this GridCell source, Direction direction,
        IEnumerable<GridCell> emptyPositions)
    {
        var row = source.Row + direction.RowOffset;
        var column = source.Column + direction.ColumnOffset;

        return emptyPositions.FirstOrDefault(x => x.Row == row && x.Column == column);
    }

}
