using System;
using Publishing.Core.Domain;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using System.Threading.Tasks;

namespace Publishing.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPrinteryRepository _printeryRepository;
        private readonly ILogger _logger;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IOrderValidator _validator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public OrderService(
            IOrderRepository orderRepository,
            IPrinteryRepository printeryRepository,
            ILogger logger,
            IPriceCalculator priceCalculator,
            IOrderValidator validator,
            IDateTimeProvider dateTimeProvider)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _printeryRepository = printeryRepository ?? throw new ArgumentNullException(nameof(printeryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _priceCalculator = priceCalculator ?? throw new ArgumentNullException(nameof(priceCalculator));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));
            _validator.Validate(dto);

            decimal price = await CalculatePriceAsync(dto.Pages, dto.Tirage);
            (DateTime start, DateTime finish) = await CalculateDatesAsync(dto.Pages, dto.Tirage);

            var order = new Order
            {
                Type = dto.Type,
                Name = dto.Name,
                Pages = dto.Pages,
                Tirage = dto.Tirage,
                DateStart = start,
                DateFinish = finish,
                Status = OrderStatus.InProgress,
                Price = price,
                PersonId = dto.PersonId,
                Printery = dto.Printery
            };

            await SaveOrderAsync(order);
            return order;
        }

        private async Task<decimal> CalculatePriceAsync(int pages, int tirage)
        {
            decimal pricePerPage = await _printeryRepository.GetPricePerPageAsync();
            return _priceCalculator.Calculate(pages, tirage, pricePerPage);
        }

        private async Task<(DateTime start, DateTime finish)> CalculateDatesAsync(int pages, int tirage)
        {
            int pagesPerDay = await _printeryRepository.GetPagesPerDayAsync();
            double days = 0;
            if (pagesPerDay > 0)
                days = Math.Ceiling((double)(pages * tirage) / pagesPerDay);

            DateTime start = _dateTimeProvider.Today.AddDays(1);
            DateTime finish = start.AddDays(days);
            return (start, finish);
        }

        private async Task SaveOrderAsync(Order order)
        {
            await _orderRepository.SaveAsync(order);
            _logger.LogInformation($"Order for product {order.Name} saved.");
        }
    }
}
