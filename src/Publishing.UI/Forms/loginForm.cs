using System;
using Publishing.Core.Interfaces;
using Publishing.Services;
using System.Windows.Forms;
using Microsoft.Extensions.Localization;

namespace Publishing
{
    public partial class loginForm : Form
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigation;
        private readonly IStringLocalizer<SharedResource> _localizer;

        [Obsolete("Designer only", error: false)]
        public loginForm()
        {
            InitializeComponent();
        }

        public loginForm(IAuthService authService, INavigationService navigation, IStringLocalizer<SharedResource> localizer)
        {
            _authService = authService;
            _navigation = navigation;
            _localizer = localizer;
            InitializeComponent();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            var user = await _authService.AuthenticateAsync(email, password).ConfigureAwait(false);

            if (user != null)
            {
                CurrentUser.UserId = user.Id;
                CurrentUser.UserType = user.Type;
                CurrentUser.UserName = user.Name;

                _navigation.Navigate<mainForm>(this);
                MessageBox.Show(_localizer["WelcomeUser", CurrentUser.UserName, CurrentUser.UserType]);
            }
            else
            {
                MessageBox.Show(_localizer["InvalidEmailOrPassword"]);
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _navigation.Navigate<registrationForm>(this);
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
