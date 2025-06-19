using System;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using System.Windows.Forms;
using Publishing.Services;

namespace Publishing
{
    public partial class addOrderForm : Form
    {
        private readonly IOrderService _orderService;
        private readonly INavigationService _navigation;
        [Obsolete("Designer only", error: false)]
        public addOrderForm()
        {
            InitializeComponent();
        }

        public addOrderForm(IOrderService orderService, INavigationService navigation)
        {
            _orderService = orderService;
            _navigation = navigation;
            InitializeComponent();
        }

        private void addOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void змінитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<profileForm>(this);
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<mainForm>(this);
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentUser.UserId = "";
            CurrentUser.UserName = "";
            CurrentUser.UserType = "";

            _navigation.Navigate<loginForm>(this);
        }

        private void addOrderForm_Load(object sender, EventArgs e)
        {
            if (CurrentUser.UserType == "контактна особа")
                організаціяToolStripMenuItem.Visible = true;
            else
                організаціяToolStripMenuItem.Visible = false;
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(pageNumTextBox.Text, out int pageNum))
            {
                MessageBox.Show("pagesNumParse");
                return;
            }

            if (!int.TryParse(tirageTextBox.Text, out int tirageNum))
            {
                MessageBox.Show("tirageParse");
                return;
            }

            var dto = new CreateOrderDto
            {
                Type = typeBox.SelectedItem?.ToString() ?? string.Empty,
                Name = nameProductTextBox.Text,
                Pages = pageNum,
                Tirage = tirageNum
            };

            var order = _orderService.CreateOrder(dto);
            totalPriceLabel.Text = "Кінцева ціна:" + order.Price.ToString();
        }

        private void orderButton_Click(object sender, EventArgs e)
        {
            string type = typeBox.SelectedItem?.ToString();
            if (type == null)
                return;

            if (!int.TryParse(pageNumTextBox.Text, out int pageNum))
            {
                MessageBox.Show("pagesNumParse");
                return;
            }

            if (!int.TryParse(tirageTextBox.Text, out int tirageNum))
            {
                MessageBox.Show("tirageParse");
                return;
            }

            var dto = new CreateOrderDto
            {
                Type = type,
                Name = nameProductTextBox.Text,
                Pages = pageNum,
                Tirage = tirageNum
            };

            var order = _orderService.CreateOrder(dto);

            MessageBox.Show("Замовлення успішно додано");
            totalPriceLabel.Text = "Кінцева ціна:" + order.Price.ToString();

            _navigation.Navigate<mainForm>(this);
        }

        private void змінитиДаніToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _navigation.Navigate<profileForm>(this);
        }

        private void додатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<addOrderForm>(this);
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<deleteOrderForm>(this);
        }

        private void організаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<organizationForm>(this);
        }
    }
}
