namespace ConsoleApp1.CodeBase.Interface
{
    public interface ISettingsMenu
    {
        void ShowSettings();
        void SetRefreshInterval(int interval);
        void SetTheme(string theme);
        int GetCurrentRefreshInterval();
        string GetTheme();
    }
}