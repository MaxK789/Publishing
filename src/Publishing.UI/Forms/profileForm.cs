using System;
using FluentValidation;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;

namespace Publishing
{
    public partial class profileForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IProfileRepository _profileRepo;
        private readonly IUserSession _session;
        private readonly IValidator<string> _validator;

        [Obsolete("Designer only", error: false)]
        public profileForm()
        {
            InitializeComponent();
        }

        public profileForm(INavigationService navigation, IProfileRepository profileRepo, IUserSession session, IValidator<string> validator)
        {
            _navigation = navigation;
            _profileRepo = profileRepo;
            _session = session;
            _validator = validator;
            InitializeComponent();
        }

        private void profileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private async void changeButton_Click(object sender, EventArgs e)
        {
            string id = _session.UserId;
            string fName = FNameTextBox.Text;
            string lName = LNameTextBox.Text;
            string email = emailTextBox.Text;
            string status = statusBox.SelectedItem?.ToString();
            string phone = phoneTextBox.Text;
            string fax = faxTextBox.Text;
            string address = addressTextBox.Text;

            bool exists = await _profileRepo.EmailExistsAsync(email).ConfigureAwait(false);
            if (exists)
            {
                MessageBox.Show("Email вже використовується");
                return;
            }

            int count = 0;
            if (fName != "")
            {
                count++;
            }
            if (lName != "")
            {
                count++;
            }
            if (email != "")
            {
                if (!_validator.Validate(email).IsValid)
                {
                    MessageBox.Show("Email is not valid");
                    return;
                }
                count++;
            }
            if (status != null)
            {
                count++;
            }
            if (phone != "")
            {
                count++;
            }
            if (fax != "")
            {
                count++;
            }
            if (address != "")
            {
                count++;
            }
            if (count > 0)
            {
                await _profileRepo.UpdateAsync(id, fName, lName, email, status, phone, fax, address).ConfigureAwait(false);

                MessageBox.Show("Дані успішно змінено");

                _navigation.Navigate<mainForm>(this);
            }
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
