using System;
using System.Data;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;

namespace Publishing
{
    public partial class deleteOrderForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IOrderRepository _orderRepo;

        [Obsolete("Designer only", error: false)]
        public deleteOrderForm()
        {
            InitializeComponent();
        }

        public deleteOrderForm(INavigationService navigation, IOrderRepository orderRepo)
        {
            _navigation = navigation;
            _orderRepo = orderRepo;
            InitializeComponent();
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["DeleteColumn"].Index && e.RowIndex >= 0)
            {
                int idToDelete = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["idOrder"].Value);

                await _orderRepo.DeleteAsync(idToDelete).ConfigureAwait(false);

                MessageBox.Show("Видалено idOrder: " + idToDelete.ToString());

                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void deleteOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<profileForm>(this);
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void вийтиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            _navigation.Navigate<loginForm>(this);
        }

        private async void deleteOrderForm_Load(object sender, EventArgs e)
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

                DataTable dataTable = await _orderRepo.GetByPersonAsync(id).ConfigureAwait(false);

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

                DataTable dataTable = await _orderRepo.GetAllAsync().ConfigureAwait(false);

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
            _navigation.Navigate<addOrderForm>(this);
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<deleteOrderForm>(this);
        }

        private void змінитиДаніToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _navigation.Navigate<profileForm>(this);
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<statisticForm>(this);
        }
    }
}
