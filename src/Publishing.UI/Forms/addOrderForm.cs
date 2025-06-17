using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Publishing
{
    public partial class addOrderForm : Form
    {
        public addOrderForm()
        {
            InitializeComponent();
        }

        private void addOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBase.CloseConnection();
            Application.Exit();
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            profileForm profForm = new profileForm();
            profForm.Show();
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainForm mainForm = new mainForm();
            mainForm.Show();
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

        private void addOrderForm_Load(object sender, EventArgs e)
        {
            if (CurrentUser.UserType == "контактна особа")
                організаціяToolStripMenuItem.Visible = true;
            else
                організаціяToolStripMenuItem.Visible = false;
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            string pages = pageNumTextBox.Text;
            if (int.TryParse(pages, out int pageNum))
            { }
            else
            {
                MessageBox.Show("pagesNumParse");
                return;
            }
            string tirage = tirageTextBox.Text;
            if (int.TryParse(tirage, out int tirageNum))
            { }
            else
            {
                MessageBox.Show("tirageParse");
                return;
            }
            string printery = printeryBox.SelectedItem?.ToString();
            if (printery == null)
            {
                return;
            }

            int totalPagesNum = pageNum * tirageNum;

            int price;

            if (int.TryParse(DataBase.ExecuteQuery("SELECT pricePerPage FROM Printery"), out int pricePerPage))
            {
                price = pricePerPage * totalPagesNum;
            }
            else
            {
                MessageBox.Show("priceParse");
                return;
            }

            totalPriceLabel.Text = "Кінцева ціна:" + price.ToString();
        }

        private void orderButton_Click(object sender, EventArgs e)
        {
            string type = typeBox.SelectedItem?.ToString();
            string nameProduct = nameProductTextBox.Text;
            string pages = pageNumTextBox.Text;
            if(int.TryParse(pages, out int pageNum))
            { }
            else
            {
                MessageBox.Show("pagesNumParse");
                return;
            }
            string tirage = tirageTextBox.Text;
            if (int.TryParse(tirage, out int tirageNum))
            { }
            else
            {
                MessageBox.Show("tirageParse");
                return;
            }
            string printery = printeryBox.SelectedItem?.ToString();
            if (type == null || printery == null)
            {
                return;
            }
            DateTime currentDate = DateTime.Now;

            DateTime startDate = currentDate.AddDays(1);


            int totalPagesNum = pageNum * tirageNum;

            List<SqlParameter> parametersForPrintery = new List<SqlParameter>
            {
                new SqlParameter("@printery", printery)
            };

            string query = "SELECT pagesPerDay FROM Printery WHERE namePrintery = @printery";
            string pagesPerDay = DataBase.ExecuteQuery(query, parametersForPrintery);

            if (int.TryParse(pagesPerDay, out int ppd))
            {
            }
            else
            {
                MessageBox.Show("pagesPerDayParse");
                return;
            }

            double num = totalPagesNum / ppd;
            if (ppd > 0)
                num = Math.Ceiling(num);            

            DateTime endDate = currentDate.AddDays(num);

            string statusOrder = "в роботі";

            int price;

            if (int.TryParse(DataBase.ExecuteQuery("SELECT pricePerPage FROM Printery"), out int pricePerPage))
            {
                price = pricePerPage * totalPagesNum;
            }
            else
            {
                MessageBox.Show("priceParse");
                return;
            }

            string idPerson = CurrentUser.UserId;

                List<SqlParameter> parametersForCheckName = new List<SqlParameter>
            {
                new SqlParameter("@type", type),
                new SqlParameter("@nameProduct", nameProduct),
                new SqlParameter("@idPerson", idPerson),
                new SqlParameter("@pages", pages)
            };

            string checkName = DataBase.ExecuteQuery("SELECT nameProduct FROM Product " +
                "WHERE typeProduct = @type AND nameProduct = @nameProduct AND idPerson = @idPerson AND " +
                "pagesNum = @pages", parametersForCheckName);

            if (!nameProduct.Equals(checkName))
            {
                List<SqlParameter> parametersForProduct = new List<SqlParameter>
                {
                    new SqlParameter("@type", type),
                    new SqlParameter("@nameProduct", nameProduct),
                    new SqlParameter("@pages", pages),
                    new SqlParameter("@idPerson", idPerson)
                };

                DataBase.ExecuteQueryWithoutResponse("INSERT INTO Product(typeProduct, idPerson, " +
                    "nameProduct, pagesNum) VALUES (@type, @idPerson, @nameProduct, @pages)", parametersForProduct);
            }

            List<SqlParameter> parametersForidProduct = new List<SqlParameter>
            {
                new SqlParameter("@type", type),
                new SqlParameter("@nameProduct", nameProduct),
                new SqlParameter("@pages", pages),
                new SqlParameter("@idPerson", idPerson)
            };

            string idProduct = DataBase.ExecuteQuery("SELECT idProduct FROM Product WHERE typeProduct = @type " +
                "AND nameProduct = @nameProduct AND idPerson = @idPerson " +
                "AND pagesNum = @pages", parametersForidProduct);


            List<SqlParameter> parametersForOrder = new List<SqlParameter>
            {
                new SqlParameter("@tirage", tirageNum),
                new SqlParameter("@pritery", printery),
                new SqlParameter("@dateOrder", currentDate),
                new SqlParameter("@dateStart", startDate),
                new SqlParameter("@dateFinish", endDate),
                new SqlParameter("@statusOrder", statusOrder),
                new SqlParameter("@price", price),
                new SqlParameter("@idProduct", idProduct),
                new SqlParameter("@idPerson", idPerson)
            };

            DataBase.ExecuteQueryWithoutResponse("INSERT INTO Orders(idProduct, idPerson, namePrintery, " +
                "dateOrder, dateStart, dateFinish, statusOrder, tirage, price) VALUES (@idProduct, @idPerson, " +
                "@pritery, @dateOrder, @dateStart, @dateFinish, @statusOrder, @tirage, @price)", parametersForOrder);

            MessageBox.Show("Замовлення успішно додано");

            this.Hide();
            mainForm mainForm = new mainForm();
            mainForm.Show();
        }

        private void змінитиДаніToolStripMenuItem_Click_1(object sender, EventArgs e)
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
            organizationForm orgF = new organizationForm();
            orgF.Show();
        }
    }
}
