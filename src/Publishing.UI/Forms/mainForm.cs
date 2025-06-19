using System;
using System.Data;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;

namespace Publishing
{
    public partial class mainForm : Form
    {
        private readonly INavigationService _navigation;
        private readonly IOrderRepository _orderRepo;

        [Obsolete("Designer only", error: false)]
        public mainForm()
        {
            InitializeComponent();
        }

        public mainForm(INavigationService navigation, IOrderRepository orderRepo)
        {
            _navigation = navigation;
            _orderRepo = orderRepo;
            InitializeComponent();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            _navigation.Navigate<loginForm>(this);
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<profileForm>(this);
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void додатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<addOrderForm>(this);
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<deleteOrderForm>(this);
        }

        private async void mainForm_Load(object sender, EventArgs e)
        {
            try
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

                await _orderRepo.UpdateExpiredAsync();
                DataTable dataTable = await _orderRepo.GetActiveAsync();

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<statisticForm>(this);
        }

        private void організаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<organizationForm>(this);
        }
    }
}
