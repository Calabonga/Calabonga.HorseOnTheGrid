namespace Calabonga.HorseOnTheGrid.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Палитра рассчитана на светлую тему, поэтому тёмную тему системы не применяем.
        UserAppTheme = AppTheme.Light;
    }

    protected override Window CreateWindow(IActivationState? activationState) =>
        new(new AppShell()) { Title = "Конь на доске" };
}
