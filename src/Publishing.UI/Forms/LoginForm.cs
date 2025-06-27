using System;
using Publishing.Core.Interfaces;
using Publishing.Services;
using System.Resources;
using System.Net.Mail;
using System.Windows.Forms;

namespace Publishing
{
    public partial class LoginForm : BaseForm
    {
        private readonly IAuthService _authService;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(Publishing.Services.IUiNotifier).Assembly);

        [Obsolete("Designer only", error: false)]
        public LoginForm()
        {
            InitializeComponent();
        }

        public LoginForm(IAuthService authService, INavigationService navigation, IUserSession session, IUiNotifier notifier)
            : base(session, navigation)
        {
            _authService = authService;
            _notifier = notifier;
            InitializeComponent();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                _notifier.NotifyWarning(_notify.GetString("EmailRequired") ?? "Email is required");
                return;
            }
            try
            {
                _ = new MailAddress(email);
            }
            catch
            {
                _notifier.NotifyWarning(_notify.GetString("InvalidEmailFormat") ?? "Invalid email format");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                _notifier.NotifyWarning(_notify.GetString("PasswordRequired") ?? "Password is required");
                return;
            }

            var result = await _authService.AuthenticateAsync(email, password);

            if (result != null)
            {
                _session.UserId = result.User.Id;
                _session.UserType = result.User.Type;
                _session.UserName = result.User.Name;
                _session.Token = result.Token;

                _navigation.Navigate<MainForm>(this);
                _notifier.NotifyInfo(string.Format(_notify.GetString("WelcomeUser") ?? "Welcome, {0}!", _session.UserName, _session.UserType));
            }
            else
            {
                _notifier.NotifyWarning(_notify.GetString("InvalidCredentials") ?? "Invalid credentials");
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _navigation.Navigate<RegistrationForm>(this);
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            PasswordTextBox.UseSystemPasswordChar = true;
        }
    }
}
