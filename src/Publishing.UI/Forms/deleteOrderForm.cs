using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Publishing
{
    public partial class deleteOrderForm : Form
    {
        public deleteOrderForm()
        {
            InitializeComponent();            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["DeleteColumn"].Index && e.RowIndex >= 0)
            {
                int idToDelete = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["idOrder"].Value);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", idToDelete)
                };

                DataBase.ExecuteQueryWithoutResponse("DELETE FROM Orders WHERE idOrder = @id", parameters);

                MessageBox.Show("Видалено idOrder: " + idToDelete.ToString());

                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void deleteOrderForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void вийтиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            this.Hide();
            loginForm logForm = new loginForm();
            logForm.Show();
        }

        private void deleteOrderForm_Load(object sender, EventArgs e)
        {
            string id = CurrentUser.UserId;

            if (CurrentUser.UserType == "контактна особа")
                організаціяToolStripMenuItem.Visible = true;
            else
                організаціяToolStripMenuItem.Visible = false;

            if (CurrentUser.UserType != "admin")
            {
                статистикаToolStripMenuItem.Visible = false;
                змінитиДаніToolStripMenuItem.Visible = true;

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", id)
                };

                DataTable dataTable = DataBase.ExecuteQueryToDataTable("SELECT * FROM Orders where idPerson = @id", parameters);

                dataGridView1.DataSource = dataTable;

                DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
                dataGridView1.Columns.Add(linkColumn);
                linkColumn.HeaderText = "Delete";
                linkColumn.Name = "DeleteColumn";
                linkColumn.Text = "Delete";
                linkColumn.UseColumnTextForLinkValue = true;

            }
            else
            {
                додатиToolStripMenuItem.Visible = false;
                статистикаToolStripMenuItem.Visible = true;
                змінитиДаніToolStripMenuItem.Visible = false;

                DataTable dataTable = DataBase.ExecuteQueryToDataTable("SELECT * FROM Orders");

                dataGridView1.DataSource = dataTable;

                DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
                dataGridView1.Columns.Add(linkColumn);
                linkColumn.HeaderText = "Delete";
                linkColumn.Name = "DeleteColumn";
                linkColumn.Text = "Delete";
                linkColumn.UseColumnTextForLinkValue = true;
            }
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

        private void змінитиДаніToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            profileForm profForm = new profileForm();
            profForm.Show();
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            statisticForm statF = new statisticForm();
            statF.Show();
        }
    }
}
