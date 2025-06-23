using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;

namespace Publishing
{
    public partial class StatisticForm : BaseForm
    {
        private readonly IRoleService _roles;
        private readonly IStatisticRepository _statRepo;
        private readonly IOrderEventsPublisher _events;
        private readonly IErrorHandler _errorHandler;

        [Obsolete("Designer only", error: false)]
        public StatisticForm()
        {
            InitializeComponent();
        }

        public StatisticForm(
            INavigationService navigation,
            IStatisticRepository statRepo,
            IUserSession session,
            IRoleService roles,
            IOrderEventsPublisher events,
            IErrorHandler errorHandler)
            : base(session, navigation)
        {
            _statRepo = statRepo;
            _roles = roles;
            _events = events;
            _errorHandler = errorHandler;
            InitializeComponent();
        }

        private async void StatisticForm_Load(object sender, EventArgs e)
        {
            _events.OrderCreated += async _ => await RefreshStatisticsAsync();
            AuthorsBox.Items.Clear();
            AuthorsBox.Items.Add("Усі");

            try
            {
                List<string[]> authorNames = await _statRepo.GetAuthorNamesAsync();

                if (authorNames != null && authorNames.Count > 0)
                {
                    foreach (string[] authorArray in authorNames)
                    {
                        string authorName = authorArray[0];
                        AuthorsBox.Items.Add(authorName);
                    }

                    AuthorsBox.SelectedIndex = 0;
                }

                OrdersChart.Series[0].Points.Clear();

                List<string[]> dataList = await _statRepo.GetOrdersPerMonthAsync();

                foreach (var dataPoint in dataList)
                {
                    OrdersChart.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
                }
            }
            catch (Exception ex)
            {
                _errorHandler.Handle(ex);
            }
        }

        private void StatisticForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void ListMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<MainForm>(this);
        }

        private void DeleteMenuItem_Click_1(object sender, EventArgs e)
        {
            _navigation.Navigate<DeleteOrderForm>(this);
        }

        private async void OrderCountPerMonthButton_Click(object sender, EventArgs e)
        {
            OrdersChart.Series[0].Points.Clear();

            List<string[]> dataList = await _statRepo.GetOrdersPerMonthAsync();

            foreach (var dataPoint in dataList)
            {
                OrdersChart.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private async void OrderCountPerAuthorButton_Click(object sender, EventArgs e)
        {
            if (AuthorsBox.SelectedItem != null && AuthorsBox.SelectedItem.ToString() == "Усі")
            {
                OrdersChart.Series[0].Points.Clear();

                List<string[]> dataList = await _statRepo.GetOrdersPerAuthorAsync();

                foreach (var dataPoint in dataList)
                {
                    OrdersChart.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
                }
            }
            else
            {
                OrdersChart.Series[0].Points.Clear();

                string fullNameAuthor = AuthorsBox.SelectedItem?.ToString();
                if (fullNameAuthor == null)
                {
                    return;
                }

                List<string[]> dataList = await _statRepo.GetOrdersPerAuthorAsync(fullNameAuthor);

                foreach (var dataPoint in dataList)
                {
                    string authorName = dataPoint[0];
                    int orderCount = int.Parse(dataPoint[1]);

                    OrdersChart.Series[0].Points.AddXY(authorName, orderCount);
                }
            }
        }

        private async void FromDateToDateButton_Click(object sender, EventArgs e)
        {
            OrdersChart.Series[0].Points.Clear();

            DateTime fDate = FromDatePicker.Value;
            DateTime lDate = ToDatePicker.Value;

            List<string[]> dataList = await _statRepo.GetOrdersPerMonthAsync(fDate, lDate);

            foreach (var dataPoint in dataList)
            {
                string orderMonth = dataPoint[0];
                int orderCount = int.Parse(dataPoint[1]);

                OrdersChart.Series[0].Points.AddXY(orderMonth, orderCount);
            }

        }

        private async void TirageButton_Click(object sender, EventArgs e)
        {
            OrdersChart.Series[0].Points.Clear();

            List<string[]> dataList = await _statRepo.GetTiragePerAuthorAsync();

            foreach (var dataPoint in dataList)
            {
                OrdersChart.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private void StatisticsMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<StatisticForm>(this);
        }

        private void ExitMenuItem_Click_1(object sender, EventArgs e)
        {
            Logout();
        }

        private async Task RefreshStatisticsAsync()
        {
            OrdersChart.Series[0].Points.Clear();
            var dataList = await _statRepo.GetOrdersPerMonthAsync();
            foreach (var dataPoint in dataList)
            {
                OrdersChart.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }
    }
}
