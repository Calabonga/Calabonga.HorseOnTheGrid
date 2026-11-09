using Calabonga.HorseOnTheGridMvvm.Game.Entities;
using System.Windows.Media;

namespace Calabonga.HorseOnTheGridMvvm.Game.Helpers;

public static class ImageHelper
{
    public static readonly Dictionary<CellType, ImageSource> CellToImage = new()
    {
        { CellType.Empty , ImageLoader.Empty},
        { CellType.Filled , ImageLoader.Filled},
        { CellType.Active , ImageLoader.Highlighted},
        { CellType.Available , ImageLoader.Available},
    };
}
