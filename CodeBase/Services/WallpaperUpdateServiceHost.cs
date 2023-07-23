using ConsoleApp1.CodeBase.Interface;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp1.CodeBase.Services
{
    public class WallpaperUpdateServiceHost : BackgroundService, IWallpaperUpdateServiceHost
    {
        private readonly IWallpaperUpdateService _wallpaperUpdateService;

        public WallpaperUpdateServiceHost(IWallpaperUpdateService wallpaperUpdateService)
        {
            _wallpaperUpdateService = wallpaperUpdateService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _wallpaperUpdateService.ExecuteAsync(stoppingToken);
        }

        async Task IWallpaperUpdateServiceHost.ExecuteAsync(CancellationToken stoppingToken)
        {
            await ExecuteAsync(stoppingToken);
        }
    }
}