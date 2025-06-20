using System;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using System.Windows.Forms;
using Publishing.Services;
using MediatR;
using Publishing.AppLayer.Commands;
using System.Resources;
using FluentValidation;
using System.Linq;

namespace Publishing
{
    public partial class addOrderForm : Form
    {
        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly IUserSession _session;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IPrinteryRepository _printeryRepository;
        private readonly ResourceManager _resources = new ResourceManager("Publishing.Resources.Resources", typeof(addOrderForm).Assembly);
        [Obsolete("Designer only", error: false)]
        public addOrderForm()
        {
            InitializeComponent();
        }

        public addOrderForm(
            IMediator mediator,
            INavigationService navigation,
            IUserSession session,
            IPriceCalculator priceCalculator,
            IPrinteryRepository printeryRepository)
        {
            _mediator = mediator;
            _navigation = navigation;
            _session = session;
            _priceCalculator = priceCalculator;
            _printeryRepository = printeryRepository;
            InitializeComponent();
        }

        private void addOrderForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _session.UserId = string.Empty;
            _session.UserName = string.Empty;
            _session.UserType = string.Empty;

            _navigation.Navigate<loginForm>(this);
        }

        private void addOrderForm_Load(object sender, EventArgs e)
        {
            if (_session.UserType == "контактна особа")
                організаціяToolStripMenuItem.Visible = true;
            else
                організаціяToolStripMenuItem.Visible = false;
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(pageNumTextBox.Text, out int pageNum))
            {
                MessageBox.Show(_resources.GetString("PagesParseError") ?? "Error");
                return;
            }

            if (!int.TryParse(tirageTextBox.Text, out int tirageNum))
            {
                MessageBox.Show(_resources.GetString("TirageParseError") ?? "Error");
                return;
            }

            decimal pricePerPage = _printeryRepository.GetPricePerPage();
            decimal price = _priceCalculator.Calculate(pageNum, tirageNum, pricePerPage);
            totalPriceLabel.Text = "Кінцева ціна:" + price.ToString();
        }

        private async void orderButton_Click(object sender, EventArgs e)
        {
            string type = typeBox.SelectedItem?.ToString();
            if (type == null)
                return;

            if (!int.TryParse(pageNumTextBox.Text, out int pageNum))
            {
                MessageBox.Show(_resources.GetString("PagesParseError") ?? "Error");
                return;
            }

            if (!int.TryParse(tirageTextBox.Text, out int tirageNum))
            {
                MessageBox.Show(_resources.GetString("TirageParseError") ?? "Error");
                return;
            }

            var dto = new CreateOrderDto
            {
                Type = type,
                Name = nameProductTextBox.Text,
                Pages = pageNum,
                Tirage = tirageNum,
                Printery = printeryBox.SelectedItem?.ToString() ?? string.Empty,
                PersonId = _session.UserId
            };

            var command = new CreateOrderCommand(dto.Type, dto.Name, dto.Pages, dto.Tirage, dto.PersonId, dto.Printery);
            try
            {
                var order = await _mediator.Send(command);

                MessageBox.Show(_resources.GetString("OrderAdded") ?? "Success");
                totalPriceLabel.Text = "Кінцева ціна:" + order.Price.ToString();

                _navigation.Navigate<mainForm>(this);
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(string.Join("\n", ex.Errors.Select(e => e.ErrorMessage)), "Validation error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
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
