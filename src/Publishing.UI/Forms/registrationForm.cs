using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using Publishing.Services;
using System.Threading.Tasks;
using System.Resources;

namespace Publishing
{
    public partial class registrationForm : BaseForm
    {
        private readonly IRegistrationService _service;
        private readonly IErrorHandler _errorHandler;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(registrationForm).Assembly);

        [Obsolete("Designer only", error: false)]
        public registrationForm()
        {
            InitializeComponent();
        }

        public registrationForm(IRegistrationService service, INavigationService navigation, IUserSession session, IErrorHandler errorHandler, IUiNotifier notifier)
            : base(session, navigation)
        {
            _service = service;
            _errorHandler = errorHandler;
            _notifier = notifier;
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
                var result = await _service.RegisterAsync(dto);
                _session.UserId = result.User.Id;
                _session.UserType = result.User.Type;
                _session.UserName = result.User.Name;
                _session.Token = result.Token;

                _navigation.Navigate<mainForm>(this);
                _notifier.NotifyInfo(string.Format(_notify.GetString("WelcomeUser") ?? "Welcome, {0}!", _session.UserName, _session.UserType));
            }
            catch (Exception ex)
            {
                _errorHandler.Handle(ex);
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
