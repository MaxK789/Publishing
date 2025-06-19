using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Publishing
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();           
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
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
            var logForm = Program.Services.GetRequiredService<loginForm>();
            logForm.Show();
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

        private void mainForm_Load(object sender, EventArgs e)
        {
            if (CurrentUser.UserType == "контактна особа")
                організаціяToolStripMenuItem.Visible = true;
            else
                організаціяToolStripMenuItem.Visible = false;

            if (CurrentUser.UserType != "admin")
            {
                статистикаToolStripMenuItem.Visible = false;
                змінитиДаніToolStripMenuItem.Visible = true;
            }
            else
            {
                статистикаToolStripMenuItem.Visible = true;
                змінитиДаніToolStripMenuItem.Visible = false;
                додатиToolStripMenuItem.Visible = false;
            }

            DataBase.ExecuteQueryWithoutResponse("UPDATE Orders SET statusOrder = 'завершено' " +
                "WHERE statusOrder <> 'завершено' AND dateFinish < GETDATE()");

            DataTable dataTable = DataBase.ExecuteQueryToDataTable("SELECT O.namePrintery, " +
                "Prod.typeProduct, Prod.nameProduct, Per.FName, Per.LName, O.dateOrder, O.dateStart, " +
                "O.dateFinish, O.statusOrder, O.price FROM(Orders O INNER JOIN Product Prod ON " +
                "Prod.idProduct = O.idProduct ) INNER JOIN Person Per ON Per.idPerson = Prod.idPerson " +
                "WHERE O.statusOrder = 'в роботі' ORDER BY O.dateOrder");

            dataGridView1.DataSource = dataTable;
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            statisticForm statF = new statisticForm();
            statF.Show();
        }

        private void організаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            organizationForm orgF = new organizationForm();
            orgF.Show();
        }
    }
}
