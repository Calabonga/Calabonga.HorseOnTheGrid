using Calabonga.HorseOnTheGridMvvm.Game.Helpers;
using Calabonga.HorseOnTheGridMvvm.UserControls;

namespace Calabonga.HorseOnTheGridMvvm.Game;

public class HorseGame
{
    public HorseGame(int rows, int columns)
    {
        PrepareCells(rows, columns);
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
}
