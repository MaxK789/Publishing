using System;
using System.Windows.Forms;

namespace Publishing.Services
{
    public class WinFormsUiNotifier : IUiNotifier, IDisposable
    {
        private readonly NotifyIcon _notifyIcon;

        public WinFormsUiNotifier()
        {
            _notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information
            };
        }

        public void NotifyInfo(string message)
        {
            Show(message, ToolTipIcon.Info);
        }

        public void NotifyWarning(string message)
        {
            Show(message, ToolTipIcon.Warning);
        }

        public void NotifyError(string message, string? details = null)
        {
            if (!string.IsNullOrEmpty(details))
            {
                try { Clipboard.SetText(details); } catch { /* ignore */ }
            }
            Show(message, ToolTipIcon.Error);
        }

        private void Show(string message, ToolTipIcon icon)
        {
            _notifyIcon.BalloonTipTitle = "Publishing";
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.BalloonTipIcon = icon;
            _notifyIcon.ShowBalloonTip(3000);
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
        }
    }
}
