using System;
using System.Data;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;

namespace Publishing
{
    public partial class MainForm : BaseForm
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IRoleService _roles;
        private readonly IErrorHandler _errorHandler;

        [Obsolete("Designer only", error: false)]
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(
            INavigationService navigation,
            IOrderRepository orderRepo,
            IUserSession session,
            IRoleService roles,
            IErrorHandler errorHandler)
            : base(session, navigation)
        {
            _orderRepo = orderRepo;
            _roles = roles;
            _errorHandler = errorHandler;
            InitializeComponent();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void PersonalDataMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<ProfileForm>(this);
        }

        private void ListMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<MainForm>(this);
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<AddOrderForm>(this);
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<DeleteOrderForm>(this);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            OrganizationMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
            if (_roles.IsAdmin(_session.UserType))
            {
                StatisticsMenuItem.Visible = true;
                PersonalDataMenuItem.Visible = false;
                AddMenuItem.Visible = false;
            }
            else
            {
                StatisticsMenuItem.Visible = false;
                PersonalDataMenuItem.Visible = true;
            }

            try
            {
                await _orderRepo.UpdateExpiredAsync();
                DataTable dataTable = await _orderRepo.GetActiveAsync();
                OrdersGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                _errorHandler.Handle(ex);
            }
        }

        private void StatisticsMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<StatisticForm>(this);
        }

        private void OrganizationMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<OrganizationForm>(this);
        }
    }
}
