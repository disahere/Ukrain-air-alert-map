using System.Runtime.InteropServices;
using ConsoleApp1.CodeBase.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp1.CodeBase.Services
{
    public class WallpaperUpdateService : IWallpaperUpdateService
    {
        private const string Url = "https://alerts.in.ua";
        private const int RefreshInterval = 60000;

        private const int SpiSetdeskwallpaper = 20;
        private const int SpifUpdateinifile = 0x01;
        private const int SpifSendchange = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    EdgeOptions options = new EdgeOptions();
                    options.AddArguments("--headless");
                    options.AddArgument("--window-size=15360,8640");

                    using (IWebDriver driver = new EdgeDriver(options))
                    {
                        driver.Navigate().GoToUrl(Url);
                        await Task.Delay(3000, stoppingToken);
                        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                        string tempImagePath = Path.ChangeExtension(Path.GetTempFileName(), "png");
                        screenshot.SaveAsFile(tempImagePath, ScreenshotImageFormat.Png);
                        SystemParametersInfo(SpiSetdeskwallpaper, 0, tempImagePath, SpifUpdateinifile | SpifSendchange);
                        File.Delete(tempImagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an error when updating the wallpaper: " + ex.Message);
                }

                await Task.Delay(RefreshInterval, stoppingToken);
            }
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