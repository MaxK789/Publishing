using System;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using System.Resources;
using System.Threading.Tasks;

namespace Publishing
{
    public partial class profileForm : BaseForm
    {
        private readonly IProfileService _profileService;
        private readonly IRoleService _roles;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(profileForm).Assembly);

        [Obsolete("Designer only", error: false)]
        public profileForm()
        {
            InitializeComponent();
        }

        public profileForm(INavigationService navigation, IProfileService profileService, IUserSession session, IRoleService roles, IUiNotifier notifier)
            : base(session, navigation)
        {
            _profileService = profileService;
            _roles = roles;
            _notifier = notifier;
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

            await _profileService.UpdateAsync(dto);
            _notifier.NotifyInfo(_notify.GetString("ProfileUpdated") ?? "Success");
            _navigation.Navigate<mainForm>(this);
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void profileForm_Load(object sender, EventArgs e)
        {
            організаціяToolStripMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
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
