using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Publishing
{
    public partial class registrationForm : Form
    {
        public registrationForm()
        {
            InitializeComponent();
        }

        private void LoginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            loginForm logForm = new loginForm();
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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 11);

            List<SqlParameter> parametersForEmail = new List<SqlParameter>
            {
                new SqlParameter("@Email", email)
            };
            string queryEmail = DataBase.ExecuteQuery("SELECT emailPerson FROM Person WHERE emailPerson = @Email", parametersForEmail);

            if(queryEmail == email)
            {
                MessageBox.Show("Email вже використовується");
                return;
            }

            if (status == null)
            {
                return;
            }

            string query = "INSERT INTO Person(FName, LName, emailPerson, typePerson) VALUES(@FName, @LName, @Email, @Status)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@FName", fName),
                new SqlParameter("@LName", lName),
                new SqlParameter("@Email", email),
                new SqlParameter("@Status", status)
            };

            DataBase.ExecuteQueryWithoutResponse(query, parameters);

            string idQuery = "SELECT MAX(idPerson) FROM Person";

            CurrentUser.UserId = DataBase.ExecuteQuery("SELECT MAX(idPerson) FROM Person");
            CurrentUser.UserType = status;
            CurrentUser.UserName = fName;

            if (int.TryParse(DataBase.ExecuteQuery(idQuery), out int id))
            {
                string queryForPasswordAndID = "INSERT INTO Pass(password, idPerson) VALUES(@password, @id)";
                List<SqlParameter> parametersForPasswordAndID = new List<SqlParameter>
                {
                    new SqlParameter("@Password", hashedPassword),
                    new SqlParameter("@id", id)
                };

                DataBase.ExecuteQueryWithoutResponse(queryForPasswordAndID, parametersForPasswordAndID);


                this.Hide();
                mainForm mainForm = new mainForm();
                mainForm.Show();
                MessageBox.Show("Вітаємо, " + CurrentUser.UserName + " (" + CurrentUser.UserType + ")!");
            }
            else
            {
                MessageBox.Show("Failed to parse the user ID.");
            }
        }

        private void registrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBase.CloseConnection();
            Application.Exit();
        }

        private void registrationForm_Load(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
        }
    }
}
