using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;

namespace Publishing
{
    public partial class profileForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IProfileRepository _profileRepo;

        [Obsolete("Designer only", error: false)]
        public profileForm()
        {
            InitializeComponent();
        }

        public profileForm(INavigationService navigation, IProfileRepository profileRepo)
        {
            _navigation = navigation;
            _profileRepo = profileRepo;
            InitializeComponent();
        }

        private void profileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private async void changeButton_Click(object sender, EventArgs e)
        {
            string id = CurrentUser.UserId;
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
                string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
                if (!Regex.IsMatch(email, pattern))
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
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            _navigation.Navigate<loginForm>(this);
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void profileForm_Load(object sender, EventArgs e)
        {
            if (CurrentUser.UserType == "контактна особа")
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
