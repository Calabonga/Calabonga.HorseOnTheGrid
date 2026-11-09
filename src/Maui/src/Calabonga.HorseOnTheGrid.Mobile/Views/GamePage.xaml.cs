using Calabonga.HorseOnTheGrid.Mobile.ViewModels;

namespace Calabonga.HorseOnTheGrid.Mobile.Views;

public partial class GamePage : ContentPage
{
    public GamePage(GameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        BoardHost.SizeChanged += OnBoardHostSizeChanged;
    }

    /// <summary>
    /// Поле должно быть квадратным на экране любого размера и при любой ориентации:
    /// сторона поля равна меньшей из сторон доступной области.
    /// Это забота представления, поэтому код находится здесь, а не в модели представления.
    /// </summary>
    private void OnBoardHostSizeChanged(object? sender, EventArgs e)
    {
        var side = Math.Min(BoardHost.Width, BoardHost.Height);
        if (side > 0)
        {
            BoardGrid.WidthRequest = side;
            BoardGrid.HeightRequest = side;
        }
    }
}
