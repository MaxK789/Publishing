using System;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using System.Resources;
using System.Threading.Tasks;

namespace Publishing
{
    public partial class ProfileForm : BaseForm
    {
        private readonly IProfileService _profileService;
        private readonly IRoleService _roles;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(Publishing.Services.IUiNotifier).Assembly);

        [Obsolete("Designer only", error: false)]
        public ProfileForm()
        {
            InitializeComponent();
        }

        public ProfileForm(INavigationService navigation, IProfileService profileService, IUserSession session, IRoleService roles, IUiNotifier notifier)
            : base(session, navigation)
        {
            _profileService = profileService;
            _roles = roles;
            _notifier = notifier;
            InitializeComponent();
        }

        private void ProfileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private async void ChangeButton_Click(object sender, EventArgs e)
        {
            var dto = new UpdateProfileDto
            {
                Id = _session.UserId,
                FirstName = FNameTextBox.Text,
                LastName = LNameTextBox.Text,
                Email = EmailTextBox.Text,
                Status = StatusBox.SelectedItem?.ToString(),
                Phone = PhoneTextBox.Text,
                Fax = FaxTextBox.Text,
                Address = AddressTextBox.Text
            };

            await _profileService.UpdateAsync(dto);
            _notifier.NotifyInfo(_notify.GetString("ProfileUpdated") ?? "Success");
            _navigation.Navigate<MainForm>(this);
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void ListMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<MainForm>(this);
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            OrganizationMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<AddOrderForm>(this);
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<DeleteOrderForm>(this);
        }

        private void PersonalDataMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<ProfileForm>(this);
        }

        private void OrganizationMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<OrganizationForm>(this);
        }
    }
}
