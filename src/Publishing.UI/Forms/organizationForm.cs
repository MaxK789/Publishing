using System;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using System.Resources;
using System.Threading.Tasks;

namespace Publishing
{
    public partial class organizationForm : BaseForm
    {
        private readonly IOrganizationService _service;
        private readonly IRoleService _roles;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _resources = new("Publishing.Resources.Resources", typeof(organizationForm).Assembly);
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(organizationForm).Assembly);

        [Obsolete("Designer only", error: false)]
        public organizationForm()
        {
            InitializeComponent();
        }

        public organizationForm(INavigationService navigation, IOrganizationService service, IUserSession session, IRoleService roles, IUiNotifier notifier)
            : base(session, navigation)
        {
            _service = service;
            _roles = roles;
            _notifier = notifier;
            InitializeComponent();
        }

        private void organizationForm_Load(object sender, EventArgs e)
        {

        }

        private void organizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private async void changeButton_Click(object sender, EventArgs e)
        {
            var dto = new UpdateOrganizationDto
            {
                Id = _session.UserId,
                Name = orgNameTextBox.Text,
                Email = emailTextBox.Text,
                Phone = phoneTextBox.Text,
                Fax = faxTextBox.Text,
                Address = addressTextBox.Text
            };

            await _service.UpdateAsync(dto);
            _notifier.NotifyInfo(_notify.GetString("OrganizationUpdated") ?? "Success");
            _navigation.Navigate<mainForm>(this);
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<profileForm>(this);
        }

        private void додатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<addOrderForm>(this);
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<deleteOrderForm>(this);
        }

        private void організаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<organizationForm>(this);
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }
    }
}
