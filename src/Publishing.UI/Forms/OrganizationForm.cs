using System;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using Publishing.Core.DTOs;
using System.Resources;
using System.Threading.Tasks;
using FluentValidation;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Publishing
{
    public partial class OrganizationForm : BaseForm
    {
        private readonly IOrganizationService _service;
        private readonly IRoleService _roles;
        private readonly IUiNotifier _notifier;
        private readonly IErrorHandler _errorHandler;
        private readonly ResourceManager _resources = new("Publishing.Resources.Resources", typeof(OrganizationForm).Assembly);
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(Publishing.Services.IUiNotifier).Assembly);

        [Obsolete("Designer only", error: false)]
        public OrganizationForm()
        {
            InitializeComponent();
        }

        public OrganizationForm(
            INavigationService navigation,
            IOrganizationService service,
            IUserSession session,
            IRoleService roles,
            IUiNotifier notifier,
            IErrorHandler errorHandler)
            : base(session, navigation)
        {
            _service = service;
            _roles = roles;
            _notifier = notifier;
            _errorHandler = errorHandler;
            InitializeComponent();
        }

        private void OrganizationForm_Load(object sender, EventArgs e)
        {

        }

        private void OrganizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private async void ChangeButton_Click(object sender, EventArgs e)
        {
            var dto = new UpdateOrganizationDto
            {
                Id = _session.UserId,
                Name = OrgNameTextBox.Text,
                Email = EmailTextBox.Text,
                Phone = PhoneTextBox.Text,
                Fax = FaxTextBox.Text,
                Address = AddressTextBox.Text
            };

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                _notifier.NotifyWarning(_notify.GetString("OrgNameRequired") ?? "Organization name is required");
                return;
            }
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                try
                {
                    _ = new MailAddress(dto.Email);
                }
                catch
                {
                    _notifier.NotifyWarning(_notify.GetString("InvalidEmailFormat") ?? "Invalid email format");
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(dto.Phone) && !Regex.IsMatch(dto.Phone, "^\\+?[1-9]\\d{2,14}$"))
            {
                _notifier.NotifyWarning(_notify.GetString("InvalidPhoneFormat") ?? "Invalid phone format");
                return;
            }
            if (!string.IsNullOrWhiteSpace(dto.Fax) && !Regex.IsMatch(dto.Fax, "^\\+?[1-9]\\d{2,14}$"))
            {
                _notifier.NotifyWarning(_notify.GetString("InvalidFaxFormat") ?? "Invalid fax format");
                return;
            }

            try
            {
                await _service.UpdateAsync(dto);
                _notifier.NotifyInfo(_notify.GetString("OrganizationUpdated") ?? "Success");
                _navigation.Navigate<MainForm>(this);
            }
            catch (ValidationException ex)
            {
                _notifier.NotifyWarning(string.Join("\n", ex.Errors.Select(e => e.ErrorMessage)));
            }
            catch (Exception ex)
            {
                _errorHandler.Handle(ex);
            }
        }

        private void ListMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<MainForm>(this);
        }

        private void PersonalDataMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<ProfileForm>(this);
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<AddOrderForm>(this);
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<DeleteOrderForm>(this);
        }

        private void OrganizationMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<OrganizationForm>(this);
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }
    }
}
