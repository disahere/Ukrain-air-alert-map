using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1.CodeBase.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace ConsoleApp1.CodeBase.Services
{
    public class WallpaperUpdateService : IWallpaperUpdateService
    {
        private int RefreshInterval { get; set; } = 2000;
        private readonly ILogger<WallpaperUpdateService> _logger;
        private CancellationTokenSource _cancellationTokenSource;
        private const string Url = "https://alerts.in.ua";

        private const int SpiSetdeskwallpaper = 20;
        private const int SpifUpdateinifile = 0x01;
        private const int SpifSendchange = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public WallpaperUpdateService(ILogger<WallpaperUpdateService> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var options = new EdgeOptions();
                    options.AddArguments("--headless");
                    options.AddArgument("--window-size=15360,8640");

                    using (var driver = new EdgeDriver(options))
                    {
                        driver.Navigate().GoToUrl(Url);
                        await Task.Delay(3000, _cancellationTokenSource.Token);
                        var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                        var tempImagePath = Path.ChangeExtension(Path.GetTempFileName(), "png");
                        screenshot.SaveAsFile(tempImagePath, ScreenshotImageFormat.Png);
                        SystemParametersInfo(SpiSetdeskwallpaper, 0, tempImagePath, SpifUpdateinifile | SpifSendchange);
                        File.Delete(tempImagePath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"There was an error when updating the wallpaper: {ex.Message}");
                }

                await Task.Delay(RefreshInterval, _cancellationTokenSource.Token);
            }

            _logger.LogInformation("Wallpaper update service is stopping...");
        }

        public void StopWallpaperUpdate()
        {
            _cancellationTokenSource?.Cancel();
        }

        public static async void StartWallpaperUpdate()
        {
            await Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IWallpaperUpdateService, WallpaperUpdateService>();
                    services.AddSingleton<IWallpaperUpdateServiceHost, WallpaperUpdateServiceHost>();
                    services.AddHostedService(provider => provider.GetRequiredService<IWallpaperUpdateServiceHost>() as BackgroundService);
                })
                .RunConsoleAsync();
        }
    }
}