using System;
using MediatR;
using Publishing.Application.Commands;
using Publishing.Core.Domain;
using Publishing.Core.Interfaces;
using FluentValidation;

namespace Publishing.Application.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPrinteryRepository _printeryRepository;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IValidator<CreateOrderCommand> _validator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            IPrinteryRepository printeryRepository,
            IPriceCalculator priceCalculator,
            IValidator<CreateOrderCommand> validator,
            IDateTimeProvider dateTimeProvider,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _printeryRepository = printeryRepository;
            _priceCalculator = priceCalculator;
            _validator = validator;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            decimal pricePerPage = _printeryRepository.GetPricePerPage();
            decimal price = _priceCalculator.Calculate(request.Pages, request.Tirage, pricePerPage);
            int pagesPerDay = _printeryRepository.GetPagesPerDay();
            double days = pagesPerDay > 0 ? Math.Ceiling((double)(request.Pages * request.Tirage) / pagesPerDay) : 0;
            DateTime start = _dateTimeProvider.Today.AddDays(1);
            DateTime finish = start.AddDays(days);

            var order = new Order
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
            return order;
        }
    }
}
