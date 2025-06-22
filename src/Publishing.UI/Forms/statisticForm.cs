using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Publishing.Services;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;
using Publishing.Services.Events;

namespace Publishing
{
    public partial class statisticForm : BaseForm
    {
        private readonly IRoleService _roles;
        private readonly IStatisticRepository _statRepo;
        private readonly IOrderEventsPublisher _events;

        [Obsolete("Designer only", error: false)]
        public statisticForm()
        {
            InitializeComponent();
        }

        public statisticForm(INavigationService navigation, IStatisticRepository statRepo, IUserSession session, IRoleService roles, IOrderEventsPublisher events)
            : base(session, navigation)
        {
            _statRepo = statRepo;
            _roles = roles;
            _events = events;
            InitializeComponent();
        }

        private async void statisticForm_Load(object sender, EventArgs e)
        {
            _events.OrderCreated += _ => _ = RefreshStatisticsAsync();
            authorsBox.Items.Clear();
            authorsBox.Items.Add("Усі");

            List<string[]> authorNames = await _statRepo.GetAuthorNamesAsync();

            if (authorNames != null && authorNames.Count > 0)
            {
                foreach (string[] authorArray in authorNames)
                {
                    string authorName = authorArray[0];
                    authorsBox.Items.Add(authorName);
                }

                authorsBox.SelectedIndex = 0;
            }

            chart1.Series[0].Points.Clear();

            List<string[]> dataList = await _statRepo.GetOrdersPerMonthAsync();

            foreach (var dataPoint in dataList)
            {
                chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private void statisticForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void видалитиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _navigation.Navigate<deleteOrderForm>(this);
        }

        private async void orderCountPerMonthButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            List<string[]> dataList = await _statRepo.GetOrdersPerMonthAsync();

            foreach (var dataPoint in dataList)
            {
                chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private async void orderCountPerAuthorButton_Click(object sender, EventArgs e)
        {
            if (authorsBox.SelectedItem != null && authorsBox.SelectedItem.ToString() == "Усі")
            {
                chart1.Series[0].Points.Clear();

                List<string[]> dataList = await _statRepo.GetOrdersPerAuthorAsync();

                foreach (var dataPoint in dataList)
                {
                    chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
                }
            }
            else
            {
                chart1.Series[0].Points.Clear();

                string fullNameAuthor = authorsBox.SelectedItem?.ToString();
                if (fullNameAuthor == null)
                {
                    return;
                }

                List<string[]> dataList = await _statRepo.GetOrdersPerAuthorAsync(fullNameAuthor);

                foreach (var dataPoint in dataList)
                {
                    string authorName = dataPoint[0];
                    int orderCount = int.Parse(dataPoint[1]);

                    chart1.Series[0].Points.AddXY(authorName, orderCount);
                }
            }
        }

        private async void fromDateToDateButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            DateTime fDate = dateTimePicker1.Value;
            DateTime lDate = dateTimePicker2.Value;

            List<string[]> dataList = await _statRepo.GetOrdersPerMonthAsync(fDate, lDate);

            foreach (var dataPoint in dataList)
            {
                string orderMonth = dataPoint[0];
                int orderCount = int.Parse(dataPoint[1]);

                chart1.Series[0].Points.AddXY(orderMonth, orderCount);
            }

        }

        private async void tirageButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            List<string[]> dataList = await _statRepo.GetTiragePerAuthorAsync();

            foreach (var dataPoint in dataList)
            {
                chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<statisticForm>(this);
        }

        private void вийтиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Logout();
        }

        private async Task RefreshStatisticsAsync()
        {
            chart1.Series[0].Points.Clear();
            var dataList = await _statRepo.GetOrdersPerMonthAsync();
            foreach (var dataPoint in dataList)
            {
                chart1.Series[0].Points.AddXY(dataPoint[0], int.Parse(dataPoint[1]));
            }
        }
    }
}
