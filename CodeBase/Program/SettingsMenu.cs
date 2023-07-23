using ConsoleApp1.CodeBase.Interface;

namespace ConsoleApp1.CodeBase.Services
{
    public class SettingsMenu : ISettingsMenu
    {
        private static SettingsMenu _instance;
        private int _refreshInterval = 6000;
        private int _currentRefreshInterval = 6000;
        private string _theme = "Light"; // Temporarily not working :)

        public static SettingsMenu GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SettingsMenu();
            }
            return _instance;
        }

        public void ShowSettings()
        {
            bool isSettingMenuOpen = true;

            while (isSettingMenuOpen)
            {
                Console.Clear();
                Console.WriteLine("Settings:");
                Console.WriteLine($"Current refresh interval: {_currentRefreshInterval} ms");
                Console.WriteLine($"Current theme: {_theme}");
                Console.WriteLine("1. Set refresh interval");
                Console.WriteLine("2. Set theme");
                Console.WriteLine("3. Back to menu");

                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowRefreshIntervalOptions();
                        break;
                    case "2":
                        ShowThemeOptions();
                        break;
                    case "3":
                        isSettingMenuOpen = false;
                        break;
                    default:
                        Console.WriteLine("Incorrect entry. Try again.");
                        Task.Delay(1500).Wait();
                        break;
                }
            }
        }

        public void SetRefreshInterval(int interval)
        {
            _refreshInterval = interval;
            _currentRefreshInterval = interval;
            Console.WriteLine($"Refresh interval set to {interval} ms.");
            Task.Delay(1500).Wait();
        }

        public int GetCurrentRefreshInterval()
        {
            return _currentRefreshInterval;
        }

        public void SetTheme(string theme)
        {
            _theme = theme;
            Console.WriteLine($"Theme set to {theme}.");
            Task.Delay(1500).Wait();
        }

        public string GetTheme()
        {
            return _theme;
        }

        private void ShowRefreshIntervalOptions()
        {
            Console.Clear();
            Console.WriteLine("Select refresh interval:");
            Console.WriteLine("1. 6000 ms");
            Console.WriteLine("2. 10000 ms");
            Console.WriteLine("3. 15000 ms");
            Console.WriteLine("4. 20000 ms");
            Console.WriteLine("5. 30000 ms");
            Console.WriteLine("6. Enter custom interval");

            Console.Write("Select an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SetRefreshInterval(6000);
                    break;
                case "2":
                    SetRefreshInterval(10000);
                    break;
                case "3":
                    SetRefreshInterval(15000);
                    break;
                case "4":
                    SetRefreshInterval(20000);
                    break;
                case "5":
                    SetRefreshInterval(30000);
                    break;
                case "6":
                    Console.Write("Enter custom interval (6000 - 60000 ms): ");
                    if (int.TryParse(Console.ReadLine(), out int customInterval) && customInterval >= 6000 && customInterval <= 60000)
                    {
                        SetRefreshInterval(customInterval);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Custom interval must be between 6000 and 60000 ms.");
                        Task.Delay(1500).Wait();
                    }
                    break;
                default:
                    Console.WriteLine("Incorrect entry. Try again.");
                    Task.Delay(1500).Wait();
                    break;
            }
        }

        private void ShowThemeOptions()
        {
            Console.Clear();
            Console.WriteLine("Select theme:");
            Console.WriteLine("1. Light");
            Console.WriteLine("2. Dark");

            Console.Write("Select an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SetTheme("Light");
                    break;
                case "2":
                    SetTheme("Dark");
                    break;
                default:
                    Console.WriteLine("Incorrect entry. Try again.");
                    Task.Delay(1500).Wait();
                    break;
            }
        }
    }
}