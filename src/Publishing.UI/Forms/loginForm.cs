using System;
using Microsoft.Data.SqlClient;
using Publishing.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Forms;

namespace Publishing
{
    public partial class loginForm : Form
    {
        private readonly IAuthService _authService;

        public loginForm(IAuthService authService)
        {
            _authService = authService;
            InitializeComponent();
            try
            {
                _authService.OpenConnection();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Не вдалося з'єднатися з базою: " + ex.Message,
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            var user = _authService.Authenticate(email, password);

            if (user != null)
            {
                CurrentUser.UserId = user.Id;
                CurrentUser.UserType = user.Type;
                CurrentUser.UserName = user.Name;

                this.Hide();
                mainForm mainForm = new mainForm();
                mainForm.Show();
                MessageBox.Show("Вітаємо, " + CurrentUser.UserName + " (" + CurrentUser.UserType + ")!");
            }
            else
            {
                MessageBox.Show("Невірна електронна пошта або пароль");
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var regForm = Program.Services.GetRequiredService<registrationForm>();
            regForm.Show();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _authService.CloseConnection();
            Application.Exit();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
