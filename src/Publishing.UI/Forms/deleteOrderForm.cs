using System;
using System.Data;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;
using System.Resources;

namespace Publishing
{
    public partial class deleteOrderForm : BaseForm
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IRoleService _roles;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(deleteOrderForm).Assembly);

        [Obsolete("Designer only", error: false)]
        public deleteOrderForm()
        {
            InitializeComponent();
        }

        public deleteOrderForm(INavigationService navigation, IOrderRepository orderRepo, IUserSession session, IRoleService roles, IUiNotifier notifier)
            : base(session, navigation)
        {
            _orderRepo = orderRepo;
            _roles = roles;
            _notifier = notifier;
            InitializeComponent();
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["DeleteColumn"].Index && e.RowIndex >= 0)
            {
                int idToDelete = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["idOrder"].Value);

                await _orderRepo.DeleteAsync(idToDelete);

                _notifier.NotifyInfo(string.Format(_notify.GetString("OrderDeleted") ?? "Deleted order {0}", idToDelete));

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
            Logout();
        }

        private async void deleteOrderForm_Load(object sender, EventArgs e)
        {
            ConfigureRoleBasedUi();
            await PopulateOrdersGrid();
        }

        private void ConfigureRoleBasedUi()
        {
            організаціяToolStripMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
            if (_roles.IsAdmin(_session.UserType))
            {
                додатиToolStripMenuItem.Visible = false;
                статистикаToolStripMenuItem.Visible = true;
                змінитиДаніToolStripMenuItem.Visible = false;
            }
            else
            {
                статистикаToolStripMenuItem.Visible = false;
                змінитиДаніToolStripMenuItem.Visible = true;
            }
        }

        private async Task PopulateOrdersGrid()
        {
            DataTable dataTable = _roles.IsAdmin(_session.UserType)
                ? await _orderRepo.GetAllAsync()
                : await _orderRepo.GetByPersonAsync(_session.UserId);
            dataGridView1.DataSource = dataTable;

            DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
            dataGridView1.Columns.Add(linkColumn);
            linkColumn.HeaderText = "Delete";
            linkColumn.Name = "DeleteColumn";
            linkColumn.Text = "Delete";
            linkColumn.UseColumnTextForLinkValue = true;
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
