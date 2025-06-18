using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.Infrastructure;
using Publishing.Infrastructure.Repositories;
using System.Windows.Forms;

namespace Publishing
{
    public partial class loginForm : Form
    {
        private readonly IOrderService _orderService;
        private readonly ILoginRepository _loginRepository;

        public loginForm(IOrderService orderService, ILoginRepository loginRepository)
        {
            _orderService = orderService;
            _loginRepository = loginRepository;
            InitializeComponent();
            try
            {
                _loginRepository.OpenConnection();
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

        public loginForm() : this(
            new OrderService(
                new OrderRepository(),
                new PrinteryRepository(),
                new LoggerService(),
                new PriceCalculator(),
                new OrderValidator(),
                new SystemDateTimeProvider()),
            new LoginRepository())
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            List<SqlParameter> parametersForPassword = new List<SqlParameter>
            {
                new SqlParameter("@Email", email)
            };

            string storedHashedPassword = _loginRepository.GetHashedPassword(email);

            if (storedHashedPassword != null && BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
            {
                string idPerson = _loginRepository.GetUserId(email);
                string typePerson = _loginRepository.GetUserType(email);
                string FName = _loginRepository.GetUserName(email);

                CurrentUser.UserId = idPerson;
                CurrentUser.UserType = typePerson;
                CurrentUser.UserName = FName;

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
            registrationForm regForm = new registrationForm();
            regForm.Show();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _loginRepository.CloseConnection();
            Application.Exit();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
