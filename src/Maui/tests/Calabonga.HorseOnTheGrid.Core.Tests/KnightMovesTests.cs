using Calabonga.HorseOnTheGrid.Core;

namespace Calabonga.HorseOnTheGrid.Core.Tests;

public class KnightMovesTests
{
    [Fact]
    public void From_TopLeftCorner_ReturnsOnlyTwoCells()
    {
        var targets = KnightMoves.From(new Position(0, 0), Board.DefaultSize).ToHashSet();

        Assert.Equal(new HashSet<Position> { new(2, 1), new(1, 2) }, targets);
    }

    [Fact]
    public void From_CenterCell_ReturnsEightCells()
    {
        var targets = KnightMoves.From(new Position(4, 4), Board.DefaultSize).ToList();

        Assert.Equal(8, targets.Count);
        Assert.All(targets, target => Assert.True(KnightMoves.IsKnightMove(new Position(4, 4), target)));
    }

    [Theory]
    [InlineData(4, 4, 5, 6, true)]
    [InlineData(4, 4, 6, 3, true)]
    [InlineData(4, 4, 2, 5, true)]
    [InlineData(4, 4, 4, 6, false)]
    [InlineData(4, 4, 5, 5, false)]
    [InlineData(4, 4, 6, 6, false)]
    [InlineData(4, 4, 4, 4, false)]
    public void IsKnightMove_ChecksLetterGShape(int fromColumn, int fromRow, int toColumn, int toRow, bool expected)
    {
        var actual = KnightMoves.IsKnightMove(new Position(fromColumn, fromRow), new Position(toColumn, toRow));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Position_ToString_ShowsNumbersStartingFromOne()
    {
        Assert.Equal("x:1, y:1", new Position(0, 0).ToString());
        Assert.Equal("x:10, y:3", new Position(9, 2).ToString());
    }
}
