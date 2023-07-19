namespace ConsoleApp1.CodeBase.Interface
{
    public interface IWallpaperUpdateService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}