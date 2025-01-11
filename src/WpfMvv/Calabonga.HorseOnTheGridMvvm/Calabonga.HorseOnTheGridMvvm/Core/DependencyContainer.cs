using Calabonga.HorseOnTheGridMvvm.ViewModels;
using Calabonga.HorseOnTheGridMvvm.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.HorseOnTheGridMvvm.Core
{
    internal static class DependencyContainer
    {
        internal static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(options =>
            {
                options.AddSerilog(dispose: true);
                options.AddDebug();
            });

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
    }
}