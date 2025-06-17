using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Publishing
{
    public partial class profileForm : Form
    {
        public profileForm()
        {
            InitializeComponent();    
        }

        private void profileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBase.CloseConnection();
            Application.Exit();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            string id = CurrentUser.UserId;
            string fName = FNameTextBox.Text;
            string lName = LNameTextBox.Text;
            string email = emailTextBox.Text;
            string status = statusBox.SelectedItem?.ToString();
            string phone = phoneTextBox.Text;
            string fax = faxTextBox.Text;
            string address = addressTextBox.Text;

            List<SqlParameter> parametersForEmail = new List<SqlParameter>
            {
                new SqlParameter("@Email", email)
            };
            string queryEmail = DataBase.ExecuteQuery("SELECT emailPerson FROM Person WHERE emailPerson = @Email", parametersForEmail);

            if (queryEmail == email)
            {
                MessageBox.Show("Email вже використовується");
                return;
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@id", id)
            };

            string query = "UPDATE Person SET";
            int count = 0;

            if (fName != "")
            {
                count++;
                query += " FName = @FName,";
                parameters.Add(new SqlParameter("@FName", fName));
            }
            if (lName != "")
            {
                count++;
                query += " LName = @LName,";
                parameters.Add(new SqlParameter("@LName", lName));
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
                query += " emailPerson = @Email,";
                parameters.Add(new SqlParameter("@Email", email));
            }
            if (status != null)
            {
                count++;
                query += " typePerson = @Status,";
                parameters.Add(new SqlParameter("@Status", status));
            }
            if (phone != "")
            {
                count++;
                query += " phonePerson = @phone,";
                parameters.Add(new SqlParameter("@phone", phone));
            }
            if (fax != "")
            {
                count++;
                query += " faxPerson = @fax,";
                parameters.Add(new SqlParameter("@fax", fax));
            }
            if (address != "")
            {
                count++;
                query += " addressPerson = @address,";
                parameters.Add(new SqlParameter("@address", address));
            }

            if (query.EndsWith(","))
            {
                count++;
                query = query.Remove(query.Length - 1, 1);
            }
            if (count > 0)
            {
                query += " WHERE idPerson = @id";

                DataBase.ExecuteQueryWithoutResponse(query, parameters);

                MessageBox.Show("Дані успішно змінено");

                this.Hide();
                mainForm mainForm = new mainForm();
                mainForm.Show();
            }           
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            this.Hide();
            loginForm logForm = new loginForm();
            logForm.Show();
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainForm mainForm = new mainForm();
            mainForm.Show();
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
            this.Hide();
            addOrderForm adF = new addOrderForm();
            adF.Show();
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            deleteOrderForm delF = new deleteOrderForm();
            delF.Show();
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            profileForm profForm = new profileForm();
            profForm.Show();
        }

        private void організаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            organizationForm orgF = new organizationForm();
            orgF.Show();
        }
    }
}
