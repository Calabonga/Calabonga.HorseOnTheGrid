using Calabonga.HorseOnTheGrid.Core;

namespace Calabonga.HorseOnTheGrid.Core.Tests;

public class GameTests
{
    /// <summary>Самый короткий путь в тупик: конь оказывается в правом верхнем углу, обе клетки рядом уже посещены.</summary>
    private static readonly Position[] PathToDeadEnd =
    [
        new(2, 1), new(4, 0), new(6, 1), new(8, 2), new(6, 3), new(7, 1), new(9, 0)
    ];

    [Fact]
    public void NewGame_KnightStandsInTopLeftCorner()
    {
        var game = new Game();

        Assert.Equal(Game.StartPosition, game.Board.Current);
        Assert.Equal(1, game.MoveCount);
        Assert.Equal(1, game.Board.GetOrder(Game.StartPosition));
        Assert.Equal(GameState.InProgress, game.State);
        Assert.False(game.CanUndo);
    }

    [Fact]
    public void TryMove_ValidMove_VisitsCell()
    {
        var game = new Game();

        var moved = game.TryMove(new Position(2, 1));

        Assert.True(moved);
        Assert.Equal(new Position(2, 1), game.Board.Current);
        Assert.Equal(2, game.Board.GetOrder(new Position(2, 1)));
        Assert.True(game.CanUndo);
    }

    [Theory]
    [InlineData(1, 1)]   // не буква «Г»
    [InlineData(0, 0)]   // клетка, где стоит конь
    [InlineData(10, 2)]  // за пределами поля
    [InlineData(-1, 2)]  // за пределами поля
    public void TryMove_InvalidMove_IsRejected(int column, int row)
    {
        var game = new Game();

        var moved = game.TryMove(new Position(column, row));

        Assert.False(moved);
        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void TryMove_ToVisitedCell_IsRejected()
    {
        var game = new Game();
        game.TryMove(new Position(2, 1));
        game.TryMove(new Position(0, 2));

        var moved = game.TryMove(new Position(2, 1)); // ходом коня достижима, но уже посещена

        Assert.False(moved);
        Assert.Equal(3, game.MoveCount);
    }

    [Fact]
    public void Undo_ReturnsKnightAndClearsCell()
    {
        var game = new Game();
        game.TryMove(new Position(2, 1));

        var undone = game.Undo();

        Assert.True(undone);
        Assert.Equal(Game.StartPosition, game.Board.Current);
        Assert.False(game.Board.IsVisited(new Position(2, 1)));
        Assert.False(game.CanUndo);
    }

    [Fact]
    public void Undo_StartCell_IsNotAllowed()
    {
        var game = new Game();

        Assert.False(game.Undo());
        Assert.Equal(1, game.MoveCount);
    }

    [Fact]
    public void DeadEnd_IsDetected_AndUndoContinuesGame()
    {
        var game = new Game();
        foreach (var position in PathToDeadEnd)
        {
            Assert.True(game.TryMove(position));
        }

        Assert.Equal(GameState.Stuck, game.State);
        Assert.Empty(game.Board.GetAvailableMoves());
        Assert.False(game.TryMove(new Position(7, 2)));

        game.Undo();

        Assert.Equal(GameState.InProgress, game.State);
    }

    [Fact]
    public void Start_ResetsBoard()
    {
        var game = new Game();
        game.TryMove(new Position(2, 1));

        game.Start();

        Assert.Equal(1, game.MoveCount);
        Assert.False(game.Board.IsVisited(new Position(2, 1)));
        Assert.False(game.CanUndo);
    }

    [Fact]
    public void Changed_IsRaisedOnMoveAndUndo()
    {
        var game = new Game();
        var raised = 0;
        game.Changed += (_, _) => raised++;

        game.TryMove(new Position(2, 1));
        game.Undo();

        Assert.Equal(2, raised);
    }

    /// <summary>
    /// Доказывает, что задача решаема: правило Варнсдорфа (идти туда, откуда меньше всего продолжений)
    /// обходит все 100 клеток, начиная с левого верхнего угла.
    /// </summary>
    [Fact]
    public void WarnsdorffRule_VisitsAllCells_AndGameIsWon()
    {
        var game = new Game();

        while (game.State == GameState.InProgress)
        {
            var next = game.Board.GetAvailableMoves()
                .MinBy(target => game.Board.GetAvailableMovesFrom(target).Count);
            Assert.True(game.TryMove(next));
        }

        Assert.Equal(GameState.Won, game.State);
        Assert.Equal(100, game.MoveCount);
    }
}
