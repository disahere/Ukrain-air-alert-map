using ConsoleApp1.CodeBase.Services;

namespace ConsoleApp1.CodeBase
{
    internal abstract class ProgramMenu
    {
        static void Main()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Launching air-raid radar");
                Console.WriteLine("2. Exit");

                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("You've chosen - Launching air-raid radar");
                        WallpaperUpdateService.StartWallpaperUpdate();
                        Console.WriteLine("Radar successfully activated.\nCurrent information every 60 seconds.\nPress any key to return to the menu...");
                        Console.ReadKey();
                        break;
                    case "2":
                        isRunning = false;
                        Console.WriteLine("The radar is complete. Press any key to exit...");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Incorrect entry. Try again.");
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}