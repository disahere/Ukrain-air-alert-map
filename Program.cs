using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace ConsoleApp1
{
    class Program
    {
        private const int SpiSetdeskwallpaper = 20;
        private const int SpifUpdateinifile = 0x01;
        private const int SpifSendchange = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SwHide = 0;

        static async Task Main()
        {
            IntPtr hWnd = GetConsoleWindow();
            ShowWindow(hWnd, SwHide);

            await Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<WallpaperUpdateService>();
                })
                .RunConsoleAsync();
        }

        public class WallpaperUpdateService : BackgroundService
        {
            private const string Url = "https://alerts.in.ua";
            private const int RefreshInterval = 60000;

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
        }
    }
}
