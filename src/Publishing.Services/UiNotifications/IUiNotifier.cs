namespace Publishing.Services
{
    public interface IUiNotifier
    {
        void NotifyInfo(string message);
        void NotifyWarning(string message);
        void NotifyError(string message, string? details = null);
    }
}
