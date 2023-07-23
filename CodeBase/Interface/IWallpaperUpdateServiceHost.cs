namespace ConsoleApp1.CodeBase.Interface
{
    public interface IWallpaperUpdateServiceHost
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}