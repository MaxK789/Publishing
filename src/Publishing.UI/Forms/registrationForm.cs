using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Services;
using FluentValidation;

namespace Publishing
{
    public partial class registrationForm : Form
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigation;
        private readonly IValidator<string> _emailValidator;

        [Obsolete("Designer only", error: false)]
        public registrationForm()
        {
            InitializeComponent();
        }

        public registrationForm(IAuthService authService, INavigationService navigation, IValidator<string> emailValidator)
        {
            _authService = authService;
            _navigation = navigation;
            _emailValidator = emailValidator;
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
            var result = _emailValidator.Validate(email);
            if (!result.IsValid)
            {
                MessageBox.Show("Email is not valid");
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
                var user = await _authService.RegisterAsync(fName, lName, email, status, password);
                CurrentUser.UserId = user.Id;
                CurrentUser.UserType = user.Type;
                CurrentUser.UserName = user.Name;

                _navigation.Navigate<mainForm>(this);
                MessageBox.Show("Вітаємо, " + CurrentUser.UserName + " (" + CurrentUser.UserType + ")!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void registrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void registrationForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
