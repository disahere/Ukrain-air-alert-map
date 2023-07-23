using ConsoleApp1.CodeBase.Interface;
using ConsoleApp1.CodeBase.Program;
using ConsoleApp1.CodeBase.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1
{
    abstract class Program
    {
        static void Main()
        {
            using var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddSingleton<ISettingsMenu, SettingsMenu>();
                    services.AddSingleton<IWallpaperUpdateService, WallpaperUpdateService>();
                    services.AddSingleton<IWallpaperUpdateServiceHost, WallpaperUpdateServiceHost>();
                    services.AddSingleton<IProgramMenu, ProgramMenu>();
                })
                .Build();

            var programMenu = host.Services.GetRequiredService<IProgramMenu>();
            programMenu.ShowMenu();
        }
    }
}