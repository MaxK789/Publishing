using System;
using Publishing.Core.Interfaces;
using Publishing.Services;
using System.Windows.Forms;

namespace Publishing
{
    public partial class loginForm : Form
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigation;
        private readonly IUserSession _session;

        [Obsolete("Designer only", error: false)]
        public loginForm()
        {
            InitializeComponent();
        }

        public loginForm(IAuthService authService, INavigationService navigation, IUserSession session)
        {
            _authService = authService;
            _navigation = navigation;
            _session = session;
            InitializeComponent();
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            var user = await _authService.AuthenticateAsync(email, password);

            if (user != null)
            {
                _session.UserId = user.Id;
                _session.UserType = user.Type;
                _session.UserName = user.Name;

                _navigation.Navigate<mainForm>(this);
                MessageBox.Show($"Вітаємо, {_session.UserName} ({_session.UserType})!");
            }
            else
            {
                MessageBox.Show("Невірна електронна пошта або пароль");
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
