using System;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using System.Resources;

namespace Publishing
{
    public partial class profileForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IProfileService _profileService;
        private readonly IUserSession _session;
        private readonly ResourceManager _resources = new ResourceManager("Publishing.UI.Resources.Resources", typeof(profileForm).Assembly);

        [Obsolete("Designer only", error: false)]
        public profileForm()
        {
            InitializeComponent();
        }

        public profileForm(INavigationService navigation, IProfileService profileService, IUserSession session)
        {
            _navigation = navigation;
            _profileService = profileService;
            _session = session;
            InitializeComponent();
        }

        private void profileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private async void changeButton_Click(object sender, EventArgs e)
        {
            var dto = new UpdateProfileDto
            {
                Id = _session.UserId,
                FirstName = FNameTextBox.Text,
                LastName = LNameTextBox.Text,
                Email = emailTextBox.Text,
                Status = statusBox.SelectedItem?.ToString(),
                Phone = phoneTextBox.Text,
                Fax = faxTextBox.Text,
                Address = addressTextBox.Text
            };

            await _profileService.UpdateAsync(dto).ConfigureAwait(false);
            MessageBox.Show(_resources.GetString("DataUpdated") ?? "Success");
            _navigation.Navigate<mainForm>(this);
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _session.UserId = string.Empty;
            _session.UserName = string.Empty;
            _session.UserType = string.Empty;

            _navigation.Navigate<loginForm>(this);
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void profileForm_Load(object sender, EventArgs e)
        {
            if (_session.UserType == "контактна особа")
                організаціяToolStripMenuItem.Visible = true;
            else
                організаціяToolStripMenuItem.Visible = false;
        }

        private void додатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<addOrderForm>(this);
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<deleteOrderForm>(this);
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<profileForm>(this);
        }

        private void організаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<organizationForm>(this);
        }
    }
}
