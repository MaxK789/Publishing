using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Publishing.AppLayer.Commands;
using Publishing.Core.Domain;
using Publishing.Core.Interfaces;
using FluentValidation;
using Publishing.Services;
using AutoMapper;
using System.Resources;

namespace Publishing.AppLayer.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPrinteryRepository _printeryRepository;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IValidator<CreateOrderCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderEventsPublisher _events;
        private readonly IUiNotifier _notifier;
        private readonly ResourceManager _resources =
            new("Publishing.Services.Resources.Notifications", typeof(IUiNotifier).Assembly);

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            IPrinteryRepository printeryRepository,
            IPriceCalculator priceCalculator,
            IValidator<CreateOrderCommand> validator,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            IUnitOfWork unitOfWork,
            IOrderEventsPublisher events,
            IUiNotifier notifier)
        {
            _orderRepository = orderRepository;
            _printeryRepository = printeryRepository;
            _priceCalculator = priceCalculator;
            _validator = validator;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
            _events = events;
            _notifier = notifier;
            _mapper = mapper;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await ValidateAsync(request, cancellationToken);

            decimal price = CalculatePrice(request);
            (DateTime start, DateTime finish) = CalculateDates(request);
            var order = BuildOrder(request, price, start, finish);

            await SaveOrderAsync(order);
            var dto = _mapper.Map<Publishing.Core.DTOs.OrderDto>(order);
            _events.PublishOrderCreated(dto);
            _notifier.NotifyInfo(_resources.GetString("OrderCreated") ?? "Order created");

            return order;
        }

        private async Task ValidateAsync(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
        }

        private decimal CalculatePrice(CreateOrderCommand request)
        {
            decimal pricePerPage = _printeryRepository.GetPricePerPage();
            return _priceCalculator.Calculate(request.Pages, request.Tirage, pricePerPage);
        }

        private (DateTime Start, DateTime Finish) CalculateDates(CreateOrderCommand request)
        {
            int pagesPerDay = _printeryRepository.GetPagesPerDay();
            double days = pagesPerDay > 0 ? Math.Ceiling((double)(request.Pages * request.Tirage) / pagesPerDay) : 0;
            DateTime start = _dateTimeProvider.Today.AddDays(1);
            DateTime finish = start.AddDays(days);
            return (start, finish);
        }

        private Order BuildOrder(CreateOrderCommand request, decimal price, DateTime start, DateTime finish)
        {
            return new Order
            {
                Type = request.Type,
                Name = request.Name,
                Pages = request.Pages,
                Tirage = request.Tirage,
                DateStart = start,
                DateFinish = finish,
                Status = OrderStatus.InProgress,
                Price = price,
                PersonId = request.PersonId,
                Printery = request.Printery
            };
        }

        private async Task SaveOrderAsync(Order order)
        {
            await _unitOfWork.BeginAsync();
            try
            {
                await _orderRepository.SaveAsync(order);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
