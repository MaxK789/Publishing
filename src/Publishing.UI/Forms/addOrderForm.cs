using System;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using System.Windows.Forms;
using MediatR;
using Publishing.AppLayer.Commands;
using System.Resources;
using FluentValidation;
using System.Linq;
using Publishing.Services;
using System.Threading.Tasks;
using AutoMapper;

namespace Publishing
{
    public partial class addOrderForm : BaseForm
    {
        private readonly IMediator _mediator;
        private readonly IOrderInputValidator _inputValidator;
        private readonly IRoleService _roles;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IPrinteryRepository _printeryRepository;
        private readonly IErrorHandler _errorHandler;
        private readonly IUiNotifier _notifier;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resources = new("Publishing.Resources.Resources", typeof(addOrderForm).Assembly);
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(Publishing.Services.IUiNotifier).Assembly);
        [Obsolete("Designer only", error: false)]
        public addOrderForm()
        {
            InitializeComponent();
        }

        public addOrderForm(
            IMediator mediator,
            INavigationService navigation,
            IUserSession session,
            IOrderInputValidator inputValidator,
            IRoleService roles,
            IPriceCalculator priceCalculator,
            IPrinteryRepository printeryRepository,
            IErrorHandler errorHandler,
            IUiNotifier notifier,
            IMapper mapper)
            : base(session, navigation)
        {
            _mediator = mediator;
            _inputValidator = inputValidator;
            _roles = roles;
            _priceCalculator = priceCalculator;
            _printeryRepository = printeryRepository;
            _errorHandler = errorHandler;
            _notifier = notifier;
            _mapper = mapper;
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
            Logout();
        }

        private void addOrderForm_Load(object sender, EventArgs e)
        {
            організаціяToolStripMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(pageNumTextBox.Text, out int pageNum))
            {
                _notifier.NotifyWarning(_resources.GetString("PagesParseError") ?? "Error");
                return;
            }

            if (!int.TryParse(tirageTextBox.Text, out int tirageNum))
            {
                _notifier.NotifyWarning(_resources.GetString("TirageParseError") ?? "Error");
                return;
            }

            decimal pricePerPage = _printeryRepository.GetPricePerPage();
            decimal price = _priceCalculator.Calculate(pageNum, tirageNum, pricePerPage);
            totalPriceLabel.Text = string.Format(_resources.GetString("TotalPriceLabel") ?? "Total: {0}", price);
        }

        private async void orderButton_Click(object sender, EventArgs e)
        {
            if (!TryParseInput(out var dto)) return;
            try
            {
                _inputValidator.Validate(dto);
                await SubmitOrder(dto);
            }
            catch (ValidationException ex)
            {
                _notifier.NotifyWarning(string.Join("\n", ex.Errors.Select(e => e.ErrorMessage)));
            }
            catch (Exception ex)
            {
                _errorHandler.Handle(ex);
            }
        }

        private bool TryParseInput(out CreateOrderDto dto)
        {
            dto = new CreateOrderDto
            {
                Type = typeBox.SelectedItem?.ToString() ?? string.Empty,
                Name = nameProductTextBox.Text,
                Printery = printeryBox.SelectedItem?.ToString() ?? string.Empty,
                PersonId = _session.UserId
            };

            if (!int.TryParse(pageNumTextBox.Text, out var pages))
            {
                _notifier.NotifyWarning(_resources.GetString("PagesParseError") ?? "Error");
                return false;
            }
            if (!int.TryParse(tirageTextBox.Text, out var tirage))
            {
                _notifier.NotifyWarning(_resources.GetString("TirageParseError") ?? "Error");
                return false;
            }
            dto.Pages = pages;
            dto.Tirage = tirage;
            return true;
        }

        private async Task SubmitOrder(CreateOrderDto dto)
        {
            var cmd = _mapper.Map<CreateOrderCommand>(dto);
            var order = await _mediator.Send(cmd);
            _notifier.NotifyInfo(_notify.GetString("OrderCreated") ?? "Success");
            totalPriceLabel.Text = string.Format(_resources.GetString("TotalPriceLabel") ?? "Total: {0}", order.Price);
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
