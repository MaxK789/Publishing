using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using Publishing.Services;

namespace Publishing
{
    public partial class registrationForm : Form
    {
        private readonly IRegistrationService _service;
        private readonly INavigationService _navigation;
        private readonly IUserSession _session;

        [Obsolete("Designer only", error: false)]
        public registrationForm()
        {
            InitializeComponent();
        }

        public registrationForm(IRegistrationService service, INavigationService navigation, IUserSession session)
        {
            _service = service;
            _navigation = navigation;
            _session = session;
            InitializeComponent();
        }


        private void LoginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _navigation.Navigate<loginForm>(this);
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            var dto = new RegisterUserDto
            {
                FirstName = FNameTextBox.Text,
                LastName = LNameTextBox.Text,
                Email = emailTextBox.Text,
                Status = statusBox.SelectedItem?.ToString(),
                Password = passwordTextBox.Text
            };

            try
            {
                var user = await _service.RegisterAsync(dto);
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
