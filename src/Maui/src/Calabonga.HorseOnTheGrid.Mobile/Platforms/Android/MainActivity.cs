using Android.App;
using Android.Content.PM;

namespace Calabonga.HorseOnTheGrid.Mobile;

// ScreenOrientation.Portrait — только портретная ориентация: квадратное поле 10×10
// занимает всю ширину экрана, а в альбомной ориентации оно стало бы слишком мелким.
[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ScreenOrientation = ScreenOrientation.Portrait,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}
