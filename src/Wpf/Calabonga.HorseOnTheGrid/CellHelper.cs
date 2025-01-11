using Calabonga.HorseOntTheGrid.Assets;
using System.Windows.Media;

namespace Calabonga.HorseOntTheGrid;

public static class CellHelper
{
    public static Dictionary<CellType, ImageSource> CellToImage = new()
    {
        { CellType.Empty, Images.Empty },
        { CellType.Available, Images.Available },
        { CellType.Filled, Images.Filled },
        { CellType.Active, Images.Highlighted },
    };
}
