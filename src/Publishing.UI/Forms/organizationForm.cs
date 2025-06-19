using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Publishing
{
    public partial class organizationForm : Form
    {
        public organizationForm()
        {
            InitializeComponent();
        }

        private void organizationForm_Load(object sender, EventArgs e)
        {

        }

        private void organizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBase.CloseConnection();
            Application.Exit();
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            string id = CurrentUser.UserId;
            string orgName = orgNameTextBox.Text;
            string email = emailTextBox.Text;            
            string phone = phoneTextBox.Text;
            string fax = faxTextBox.Text;
            string address = addressTextBox.Text;

            List<SqlParameter> parametersForCheckName = new List<SqlParameter>
            {
                new SqlParameter("@orgName", orgName)
            };

            string checkName = DataBase.ExecuteQuery("SELECT nameOrganization FROM Organization " +
                "WHERE nameOrganization = @orgName", parametersForCheckName);

            if (!orgName.Equals(checkName))
            {
                string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
                if (!Regex.IsMatch(email, pattern))
                {
                    MessageBox.Show("Email is not valid");
                    return;
                }

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@orgName", orgName),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@phone", phone),
                    new SqlParameter("@fax", fax),
                    new SqlParameter("@address", address),
                    new SqlParameter("@id", id)
                };

                DataBase.ExecuteQueryWithoutResponse("INSERT INTO Organization(nameOrganization, emailOrganization, " +
                    "phoneOrganization, faxOrganization, addressOrganization, idPerson) VALUES (@orgName, @Email," +
                    " @phone, @fax, @address, @id)", parameters);
            }
            else
            {
                List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@id", id)
            };

                string query = "UPDATE Organization SET";
                int count = 0;

                if (orgName != "")
                {
                    count++;
                    query += " nameOrganization = @orgName,";
                    parameters.Add(new SqlParameter("@orgName", orgName));
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
                    query += " emailOrganization = @Email,";
                    parameters.Add(new SqlParameter("@Email", email));
                }
                if (phone != "")
                {
                    count++;
                    query += " phoneOrganization = @phone,";
                    parameters.Add(new SqlParameter("@phone", phone));
                }
                if (fax != "")
                {
                    count++;
                    query += " faxOrganization = @fax,";
                    parameters.Add(new SqlParameter("@fax", fax));
                }
                if (address != "")
                {
                    count++;
                    query += " addressOrganization = @address,";
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
                }
                MessageBox.Show("Дані успішно змінено");

                this.Hide();
                mainForm mainForm = new mainForm();
                mainForm.Show();
            }
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainForm mainForm = new mainForm();
            mainForm.Show();
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            profileForm profForm = new profileForm();
            profForm.Show();
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

        private void організаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var orgF = Program.Services.GetRequiredService<organizationForm>();
            orgF.Show();
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            this.Hide();
            var logForm = Program.Services.GetRequiredService<loginForm>();
            logForm.Show();
        }
    }
}
