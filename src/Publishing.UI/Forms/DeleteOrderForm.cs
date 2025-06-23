using System;
using System.Data;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;
using System.Resources;

namespace Publishing
{
    public partial class DeleteOrderForm : BaseForm
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IRoleService _roles;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(Publishing.Services.IUiNotifier).Assembly);

        [Obsolete("Designer only", error: false)]
        public DeleteOrderForm()
        {
            InitializeComponent();
        }

        public DeleteOrderForm(INavigationService navigation, IOrderRepository orderRepo, IUserSession session, IRoleService roles, IUiNotifier notifier)
            : base(session, navigation)
        {
            _orderRepo = orderRepo;
            _roles = roles;
            _notifier = notifier;
            InitializeComponent();
        }

        private async void OrdersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == OrdersGridView.Columns["DeleteColumn"].Index && e.RowIndex >= 0)
            {
                int idToDelete = Convert.ToInt32(OrdersGridView.Rows[e.RowIndex].Cells["idOrder"].Value);

                await _orderRepo.DeleteAsync(idToDelete);

                _notifier.NotifyInfo(string.Format(_notify.GetString("OrderDeleted") ?? "Deleted order {0}", idToDelete));

                OrdersGridView.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void DeleteOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void PersonalDataMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<ProfileForm>(this);
        }

        private void ListMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<MainForm>(this);
        }

        private void ExitMenuItem_Click_1(object sender, EventArgs e)
        {
            Logout();
        }

        private async void DeleteOrderForm_Load(object sender, EventArgs e)
        {
            ConfigureRoleBasedUi();
            await PopulateOrdersGrid();
        }

        private void ConfigureRoleBasedUi()
        {
            OrganizationMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
            if (_roles.IsAdmin(_session.UserType))
            {
                AddMenuItem.Visible = false;
                StatisticsMenuItem.Visible = true;
                PersonalDataMenuItem.Visible = false;
            }
            else
            {
                StatisticsMenuItem.Visible = false;
                PersonalDataMenuItem.Visible = true;
            }
        }

        private async Task PopulateOrdersGrid()
        {
            DataTable dataTable = _roles.IsAdmin(_session.UserType)
                ? await _orderRepo.GetAllAsync()
                : await _orderRepo.GetByPersonAsync(_session.UserId);
            OrdersGridView.DataSource = dataTable;

            DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
            OrdersGridView.Columns.Add(linkColumn);
            linkColumn.HeaderText = "Delete";
            linkColumn.Name = "DeleteColumn";
            linkColumn.Text = "Delete";
            linkColumn.UseColumnTextForLinkValue = true;
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<AddOrderForm>(this);
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<DeleteOrderForm>(this);
        }

        private void PersonalDataMenuItem_Click_1(object sender, EventArgs e)
        {
            _navigation.Navigate<ProfileForm>(this);
        }

        private void StatisticsMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<StatisticForm>(this);
        }
    }
}
