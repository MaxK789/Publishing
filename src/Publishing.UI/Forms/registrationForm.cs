using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Services;
using Microsoft.Extensions.Localization;

namespace Publishing
{
    public partial class registrationForm : Form
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigation;
        private readonly IStringLocalizer<SharedResource> _localizer;

        [Obsolete("Designer only", error: false)]
        public registrationForm()
        {
            InitializeComponent();
        }

        public registrationForm(IAuthService authService, INavigationService navigation, IStringLocalizer<SharedResource> localizer)
        {
            _authService = authService;
            _navigation = navigation;
            _localizer = localizer;
            InitializeComponent();
        }


        private void LoginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _navigation.Navigate<loginForm>(this);
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            string fName = FNameTextBox.Text;
            string lName = LNameTextBox.Text;
            string email = emailTextBox.Text;
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            if (!Regex.IsMatch(email, pattern))
            {
                MessageBox.Show(_localizer["InvalidEmail"]);
                return;
            }
            string status = statusBox.SelectedItem?.ToString();
            string password = passwordTextBox.Text;

            if (status == null)
            {
                return;
            }

            try
            {
                var user = await _authService.RegisterAsync(fName, lName, email, status, password).ConfigureAwait(false);
                CurrentUser.UserId = user.Id;
                CurrentUser.UserType = user.Type;
                CurrentUser.UserName = user.Name;

                _navigation.Navigate<mainForm>(this);
                MessageBox.Show(_localizer["WelcomeUser", CurrentUser.UserName, CurrentUser.UserType]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void registrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void registrationForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
