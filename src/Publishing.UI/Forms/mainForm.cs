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
        private readonly IUserSession _session;

        [Obsolete("Designer only", error: false)]
        public mainForm()
        {
            InitializeComponent();
        }

        public mainForm(INavigationService navigation, IOrderRepository orderRepo, IUserSession session)
        {
            _navigation = navigation;
            _orderRepo = orderRepo;
            _session = session;
            InitializeComponent();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _session.UserId = string.Empty;
            _session.UserName = string.Empty;
            _session.UserType = string.Empty;

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
            if (_session.UserType == "контактна особа")
                організаціяToolStripMenuItem.Visible = true;
            else
                організаціяToolStripMenuItem.Visible = false;

            if (_session.UserType != "admin")
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

            await _orderRepo.UpdateExpiredAsync().ConfigureAwait(false);
            DataTable dataTable = await _orderRepo.GetActiveAsync().ConfigureAwait(false);

            dataGridView1.DataSource = dataTable;
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
