using System;
using Publishing.Core.Interfaces;
using Publishing.Services;
using System.Resources;
using System.Windows.Forms;

namespace Publishing
{
    public partial class loginForm : BaseForm
    {
        private readonly IAuthService _authService;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(loginForm).Assembly);

        [Obsolete("Designer only", error: false)]
        public loginForm()
        {
            InitializeComponent();
        }

        public loginForm(IAuthService authService, INavigationService navigation, IUserSession session, IUiNotifier notifier)
            : base(session, navigation)
        {
            _authService = authService;
            _notifier = notifier;
            InitializeComponent();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            var result = await _authService.AuthenticateAsync(email, password);

            if (result != null)
            {
                _session.UserId = result.User.Id;
                _session.UserType = result.User.Type;
                _session.UserName = result.User.Name;
                _session.Token = result.Token;

                _navigation.Navigate<mainForm>(this);
                _notifier.NotifyInfo(string.Format(_notify.GetString("WelcomeUser") ?? "Welcome, {0}!", _session.UserName, _session.UserType));
            }
            else
            {
                _notifier.NotifyWarning(_notify.GetString("InvalidCredentials") ?? "Invalid credentials");
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _navigation.Navigate<registrationForm>(this);
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
