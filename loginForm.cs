using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Publishing
{
    public partial class loginForm : Form
    {
        public loginForm()
        {
            InitializeComponent();
            DataBase.OpenConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordTextBox.Text;

            List<SqlParameter> parametersForPassword = new List<SqlParameter>
            {
                new SqlParameter("@Email", email)
            };

            string storedHashedPassword = DataBase.ExecuteQuery("SELECT password FROM Person INNER JOIN Pass ON Pass.idPerson = Person.idPerson WHERE emailPerson = @Email", parametersForPassword);

            if (storedHashedPassword != null && BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
            {
                List<SqlParameter> parametersForId = new List<SqlParameter>
                {
                    new SqlParameter("@Email", email)
                };

                List<SqlParameter> parametersForType = new List<SqlParameter>
                {
                    new SqlParameter("@Email", email)
                };

                List<SqlParameter> parametersForFName = new List<SqlParameter>
                {
                    new SqlParameter("@Email", email)
                };

                string idPerson = DataBase.ExecuteQuery("SELECT idPerson FROM Person WHERE emailPerson = @Email", parametersForId);
                string typePerson = DataBase.ExecuteQuery("SELECT typePerson FROM Person WHERE emailPerson = @Email", parametersForType);
                string FName = DataBase.ExecuteQuery("SELECT FName FROM Person WHERE emailPerson = @Email", parametersForFName);

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
            DataBase.CloseConnection();
            Application.Exit();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
