using System;
using System.Collections.Generic;
using FluentValidation;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Services;

namespace Publishing
{
    public partial class registrationForm : Form
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigation;
        private readonly IUserSession _session;
        private readonly IValidator<string> _validator;

        [Obsolete("Designer only", error: false)]
        public registrationForm()
        {
            InitializeComponent();
        }

        public registrationForm(IAuthService authService, INavigationService navigation, IUserSession session, IValidator<string> validator)
        {
            _authService = authService;
            _navigation = navigation;
            _session = session;
            _validator = validator;
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
            if (!_validator.Validate(email).IsValid)
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
                _session.UserId = user.Id;
                _session.UserType = user.Type;
                _session.UserName = user.Name;

                _navigation.Navigate<mainForm>(this);
                MessageBox.Show($"Вітаємо, {_session.UserName} ({_session.UserType})!");
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
