using Calabonga.HorseOnTheGrid.Core;
using Calabonga.HorseOnTheGrid.Mobile.ViewModels;
using Calabonga.HorseOnTheGrid.Mobile.Views;
using Microsoft.Extensions.Logging;

namespace Calabonga.HorseOnTheGrid.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                // Псевдонимы AppRegular и AppSemibold используются в стилях.
                // Чтобы перейти на Source Sans 3 из брендбука, положите файлы шрифта
                // в Resources/Fonts и замените здесь только имена файлов.
                fonts.AddFont("OpenSans-Regular.ttf", "AppRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "AppSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Внедрение зависимостей: ядро игры, хранилище настроек, модель представления и страница.
        builder.Services.AddSingleton(Preferences.Default);
        builder.Services.AddTransient(_ => new Game());
        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<GamePage>();

        return builder.Build();
    }
}
