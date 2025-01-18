using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Calabonga.HorseOnTheGridMvvm.Game.Helpers;

public static class ImageHelper
{
    public static readonly ImageSource Empty = LoadImage("CellEmpty.png");
    public static readonly ImageSource Filled = LoadImage("CellFilled.png");
    public static readonly ImageSource Highlighted = LoadImage("CellHighlight.png");
    public static readonly ImageSource Available = LoadImage("CellAvailable.png");

    private static ImageSource LoadImage(string fileName)
    {
        return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
    }

}
