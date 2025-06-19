using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Publishing
{
    public partial class registrationForm : Form
    {
        private readonly IAuthService _authService;

        public registrationForm(IAuthService authService)
        {
            _authService = authService;
            InitializeComponent();
            try
            {
                _authService.OpenConnection();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Не вдалося з'єднатися з базою: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void LoginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var logForm = Program.Services.GetRequiredService<loginForm>();
            logForm.Show();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string fName = FNameTextBox.Text;
            string lName = LNameTextBox.Text;
            string email = emailTextBox.Text;
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            if (!Regex.IsMatch(email, pattern))
            {
                MessageBox.Show("Email is not valid");
                return;
            }
            string status = statusBox.SelectedItem?.ToString();
            string password = passwordTextBox.Text;

            if (status == null)
            {
                return;
            }

            try
            {
                var user = _authService.Register(fName, lName, email, status, password);
                CurrentUser.UserId = user.Id;
                CurrentUser.UserType = user.Type;
                CurrentUser.UserName = user.Name;

                this.Hide();
                mainForm mainForm = new mainForm();
                mainForm.Show();
                MessageBox.Show("Вітаємо, " + CurrentUser.UserName + " (" + CurrentUser.UserType + ")!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void registrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _authService.CloseConnection();
            Application.Exit();
        }

        private void registrationForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
