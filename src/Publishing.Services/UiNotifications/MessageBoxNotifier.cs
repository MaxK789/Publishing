#if WINDOWS
using System.Windows.Forms;

namespace Publishing.Services
{
    // Displays notifications using standard message boxes instead of tray balloons
    // PUB002 relates to MessageBox.Show usage which is allowed here by design
#pragma warning disable PUB002
    public class MessageBoxNotifier : IUiNotifier
    {
        private const string Title = "Publishing";

        public void NotifyInfo(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void NotifyWarning(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void NotifyError(string message, string? details = null)
        {
            if (!string.IsNullOrEmpty(details))
            {
                try { Clipboard.SetText(details); } catch { /* ignore */ }
            }
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
#pragma warning restore PUB002
}
#endif
