using ConsoleApp1.CodeBase.Interface;

namespace ConsoleApp1.CodeBase.Program
{
    internal class ProgramMenu : IProgramMenu
    {
        private readonly ISettingsMenu _settingsMenu;
        private readonly IWallpaperUpdateService _wallpaperUpdateService;

        public ProgramMenu(ISettingsMenu settingsMenu, IWallpaperUpdateService wallpaperUpdateService)
        {
            _settingsMenu = settingsMenu;
            _wallpaperUpdateService = wallpaperUpdateService;
        }

        public void ShowMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Launching air-raid radar");
                Console.WriteLine("2. Stop air-raid radar");
                Console.WriteLine("3. Settings");
                Console.WriteLine("4. Exit");

                Console.Write("Select an option: ");
                string input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("You've chosen - Launching air-raid radar");
                        _wallpaperUpdateService.ExecuteAsync(CancellationToken.None);
                        int currentInterval = _settingsMenu.GetCurrentRefreshInterval();
                        string theme = _settingsMenu.GetTheme();
                        Console.WriteLine($"Radar successfully activated.\nCurrent information every {currentInterval / 1000} seconds.\nTheme: {theme}");
                        Task.Delay(5000).Wait();
                        break;
                    case "2":
                        Console.WriteLine("You've chosen - Stop air-raid radar");
                        _wallpaperUpdateService.StopWallpaperUpdate();
                        Console.WriteLine("Radar successfully stopped.");
                        Task.Delay(1500).Wait();
                        break;
                    case "3":
                        _settingsMenu.ShowSettings();
                        break;
                    case "4":
                        isRunning = false;
                        _wallpaperUpdateService.StopWallpaperUpdate();
                        Console.WriteLine("The radar is complete.");
                        Task.Delay(1500).Wait();
                        break;
                    default:
                        Console.WriteLine("Incorrect entry. Try again.");
                        Task.Delay(1500).Wait();
                        break;
                }
            }
        }
    }
}