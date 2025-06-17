using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Publishing
{
    public partial class statisticForm : Form
    {
        public statisticForm()
        {
            InitializeComponent();
        }

        private void statisticForm_Load(object sender, EventArgs e)
        {
            authorsBox.Items.Clear();
            authorsBox.Items.Add("Усі");

            List<string[]> authorNames = DataBase.ExecuteQueryList("SELECT DISTINCT (FName + ' ' + LName) " +
                "AS Author FROM Person P INNER JOIN Orders O ON O.idPerson = P.idPerson");

            if (authorNames != null && authorNames.Count > 0)
            {
                foreach (string[] authorArray in authorNames)
                {
                    string authorName = authorArray[0];
                    authorsBox.Items.Add(authorName);
                }

                authorsBox.SelectedIndex = 0;
            }

            chart1.Series[0].Points.Clear();

            List<string[]> dataList = DataBase.ExecuteQueryList("SELECT DATENAME(MONTH, dateOrder) AS orderMonth, " +
                "COUNT(*) AS Number FROM Orders WHERE YEAR(dateOrder) = YEAR(GETDATE()) " +
                "GROUP BY DATENAME(MONTH, dateOrder)");

            foreach (var dataPoint in dataList)
            {
                chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private void statisticForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBase.CloseConnection();
            Application.Exit();
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

        private void видалитиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            deleteOrderForm delF = new deleteOrderForm();
            delF.Show();
        }

        private void orderCountPerMonthButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            List<string[]> dataList = DataBase.ExecuteQueryList("SELECT DATENAME(MONTH, dateOrder) AS orderMonth, " +
                "COUNT(*) AS Number FROM Orders WHERE YEAR(dateOrder) = YEAR(GETDATE()) " +
                "GROUP BY DATENAME(MONTH, dateOrder)");

            foreach (var dataPoint in dataList)
            {
                chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private void orderCountPerAuthorButton_Click(object sender, EventArgs e)
        {
            if (authorsBox.SelectedItem != null && authorsBox.SelectedItem.ToString() == "Усі")
            {
                chart1.Series[0].Points.Clear();

                List<string[]> dataList = DataBase.ExecuteQueryList("SELECT (P.FName + ' ' + P.LName) AS Author, " +
                    "COUNT(*) AS Number\r\nFROM Orders O INNER JOIN Person P ON P.idPerson = O.idPerson\r\n" +
                    "GROUP BY (P.FName + ' ' + P.LName)");

                foreach (var dataPoint in dataList)
                {
                    chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
                }
            }
            else
            {
                chart1.Series[0].Points.Clear();

                string fullNameAuthor = authorsBox.SelectedItem?.ToString();
                if (fullNameAuthor == null)
                {
                    return;
                }

                List<SqlParameter> parameters = new List<SqlParameter>
{
    new SqlParameter("@fullNameAuthor", fullNameAuthor)
};
                List<string[]> dataList = DataBase.ExecuteQueryList("SELECT (P.FName + ' ' + P.LName) AS Author, " +
                    "COUNT(*) AS Number FROM Orders O INNER JOIN Person P ON P.idPerson = O.idPerson " +
                    "WHERE (P.FName + ' ' + P.LName) = @fullNameAuthor GROUP BY (P.FName + ' ' + P.LName)", parameters);

                foreach (var dataPoint in dataList)
                {
                    string authorName = dataPoint[0];
                    int orderCount = int.Parse(dataPoint[1]);

                    chart1.Series[0].Points.AddXY(authorName, orderCount);
                }
            }
        }

        private void fromDateToDateButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            DateTime fDate = dateTimePicker1.Value;
            DateTime lDate = dateTimePicker2.Value;

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StartDate", fDate),
                new SqlParameter("@EndDate", lDate)
            };

            List<string[]> dataList = DataBase.ExecuteQueryList(
                "SELECT DATENAME(MONTH, dateOrder) AS orderMonth, COUNT(*) AS Number " +
                "FROM Orders O INNER JOIN Person P ON P.idPerson = O.idPerson " +
                "WHERE dateOrder BETWEEN @StartDate AND @EndDate " +
                "GROUP BY DATENAME(MONTH, dateOrder)", parameters);

            foreach (var dataPoint in dataList)
            {
                string orderMonth = dataPoint[0];
                int orderCount = int.Parse(dataPoint[1]);

                chart1.Series[0].Points.AddXY(orderMonth, orderCount);
            }

        }

        private void tirageButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            List<string[]> dataList = DataBase.ExecuteQueryList("SELECT (P.FName + ' ' + P.LName) " +
                "AS Author, Sum(tirage) AS sumTirage\r\nFROM Orders O INNER JOIN Person P " +
                "ON P.idPerson = O.idPerson\r\nGROUP BY (P.FName + ' ' + P.LName)");

            foreach (var dataPoint in dataList)
            {
                chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            statisticForm statF = new statisticForm();
            statF.Show();
        }

        private void вийтиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            this.Hide();
            loginForm logForm = new loginForm();
            logForm.Show();
        }
    }
}
