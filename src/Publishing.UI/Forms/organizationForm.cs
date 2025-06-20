using System;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using FluentValidation;

namespace Publishing
{
    public partial class organizationForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IOrganizationRepository _orgRepo;
        private readonly IValidator<string> _emailValidator;

        [Obsolete("Designer only", error: false)]
        public organizationForm()
        {
            InitializeComponent();
        }

        public organizationForm(INavigationService navigation, IOrganizationRepository orgRepo, IValidator<string> emailValidator)
        {
            _navigation = navigation;
            _orgRepo = orgRepo;
            _emailValidator = emailValidator;
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
            string id = CurrentUser.UserId;
            string orgName = orgNameTextBox.Text;
            string email = emailTextBox.Text;            
            string phone = phoneTextBox.Text;
            string fax = faxTextBox.Text;
            string address = addressTextBox.Text;

            string? checkName = await _orgRepo.GetNameIfExistsAsync(orgName).ConfigureAwait(false);

            if (!orgName.Equals(checkName))
            {
                var result = _emailValidator.Validate(email);
                if (!result.IsValid)
                {
                    MessageBox.Show("Email is not valid");
                    return;
                }

                await _orgRepo.InsertAsync(orgName, email, phone, fax, address, id).ConfigureAwait(false);
            }
            else
            {
                await _orgRepo.UpdateAsync(id, orgName, email, phone, fax, address).ConfigureAwait(false);
                MessageBox.Show("Дані успішно змінено");

                _navigation.Navigate<mainForm>(this);
            }
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
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            _navigation.Navigate<loginForm>(this);
        }
    }
}
