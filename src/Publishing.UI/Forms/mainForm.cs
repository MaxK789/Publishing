using System;
using System.Data;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;

namespace Publishing
{
    public partial class mainForm : BaseForm
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IRoleService _roles;

        [Obsolete("Designer only", error: false)]
        public mainForm()
        {
            InitializeComponent();
        }

        public mainForm(INavigationService navigation, IOrderRepository orderRepo, IUserSession session, IRoleService roles)
            : base(session, navigation)
        {
            _orderRepo = orderRepo;
            _roles = roles;
            InitializeComponent();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
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
            організаціяToolStripMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
            if (_roles.IsAdmin(_session.UserType))
            {
                статистикаToolStripMenuItem.Visible = true;
                змінитиДаніToolStripMenuItem.Visible = false;
                додатиToolStripMenuItem.Visible = false;
            }
            else
            {
                статистикаToolStripMenuItem.Visible = false;
                змінитиДаніToolStripMenuItem.Visible = true;
            }

            await _orderRepo.UpdateExpiredAsync();
            DataTable dataTable = await _orderRepo.GetActiveAsync();

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
