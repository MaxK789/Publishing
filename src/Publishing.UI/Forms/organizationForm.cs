using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;

namespace Publishing
{
    public partial class organizationForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IOrganizationRepository _orgRepo;

        [Obsolete("Designer only", error: false)]
        public organizationForm()
        {
            InitializeComponent();
        }

        public organizationForm(INavigationService navigation, IOrganizationRepository orgRepo)
        {
            _navigation = navigation;
            _orgRepo = orgRepo;
            InitializeComponent();
        }

        private void organizationForm_Load(object sender, EventArgs e)
        {

        }

        private void organizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private async void changeButton_Click(object sender, EventArgs e)
        {
            string id = CurrentUser.UserId;
            string orgName = orgNameTextBox.Text;
            string email = emailTextBox.Text;            
            string phone = phoneTextBox.Text;
            string fax = faxTextBox.Text;
            string address = addressTextBox.Text;

            string? checkName = await _orgRepo.GetNameIfExistsAsync(orgName);

            if (!orgName.Equals(checkName))
            {
                string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
                if (!Regex.IsMatch(email, pattern))
                {
                    MessageBox.Show("Email is not valid");
                    return;
                }

                await _orgRepo.InsertAsync(orgName, email, phone, fax, address, id);
            }
            else
            {
                await _orgRepo.UpdateAsync(id, orgName, email, phone, fax, address);
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
