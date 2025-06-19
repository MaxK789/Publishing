using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;

namespace Publishing
{
    public partial class organizationForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IDatabaseClient _db;

        [Obsolete("Designer only", error: false)]
        public organizationForm()
        {
            InitializeComponent();
        }

        public organizationForm(INavigationService navigation, IDatabaseClient db)
        {
            _navigation = navigation;
            _db = db;
            InitializeComponent();
        }

        private void organizationForm_Load(object sender, EventArgs e)
        {

        }

        private void organizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
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

            string checkName = _db.ExecuteQuery("SELECT nameOrganization FROM Organization " +
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

                _db.ExecuteQueryWithoutResponse("INSERT INTO Organization(nameOrganization, emailOrganization, " +
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

                    _db.ExecuteQueryWithoutResponse(query, parameters);
                }
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
