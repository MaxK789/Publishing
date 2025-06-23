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
    public partial class AddOrderForm : BaseForm
    {
        private readonly IMediator _mediator;
        private readonly IOrderInputValidator _inputValidator;
        private readonly IRoleService _roles;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IPrinteryRepository _printeryRepository;
        private readonly IErrorHandler _errorHandler;
        private readonly IUiNotifier _notifier;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resources = new("Publishing.Resources.Resources", typeof(AddOrderForm).Assembly);
        private readonly ResourceManager _notify = new("Publishing.Services.Resources.Notifications", typeof(Publishing.Services.IUiNotifier).Assembly);
        [Obsolete("Designer only", error: false)]
        public AddOrderForm()
        {
            InitializeComponent();
        }

        public AddOrderForm(
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

        private void AddOrderForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void AddOrderForm_Load(object sender, EventArgs e)
        {
            OrganizationMenuItem.Visible = _roles.IsContactPerson(_session.UserType);
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(PageNumTextBox.Text, out int pageNum))
            {
                _notifier.NotifyWarning(_resources.GetString("PagesParseError") ?? "Error");
                return;
            }

            if (!int.TryParse(TirageTextBox.Text, out int tirageNum))
            {
                _notifier.NotifyWarning(_resources.GetString("TirageParseError") ?? "Error");
                return;
            }

            decimal pricePerPage = _printeryRepository.GetPricePerPage();
            decimal price = _priceCalculator.Calculate(pageNum, tirageNum, pricePerPage);
            TotalPriceLabel.Text = string.Format(_resources.GetString("TotalPriceLabel") ?? "Total: {0}", price);
        }

        private async void OrderButton_Click(object sender, EventArgs e)
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
                Type = TypeComboBox.SelectedItem?.ToString() ?? string.Empty,
                Name = NameProductTextBox.Text,
                Printery = PrinteryComboBox.SelectedItem?.ToString() ?? string.Empty,
                PersonId = _session.UserId
            };

            if (!int.TryParse(PageNumTextBox.Text, out var pages))
            {
                _notifier.NotifyWarning(_resources.GetString("PagesParseError") ?? "Error");
                return false;
            }
            if (!int.TryParse(TirageTextBox.Text, out var tirage))
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
            TotalPriceLabel.Text = string.Format(_resources.GetString("TotalPriceLabel") ?? "Total: {0}", order.Price);
            _navigation.Navigate<MainForm>(this);
        }

        private void PersonalDataMenuItem_Click_1(object sender, EventArgs e)
        {
            _navigation.Navigate<ProfileForm>(this);
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<AddOrderForm>(this);
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<DeleteOrderForm>(this);
        }

        private void OrganizationMenuItem_Click(object sender, EventArgs e)
        {
            _navigation.Navigate<OrganizationForm>(this);
        }
    }
}
