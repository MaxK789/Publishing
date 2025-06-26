using MediatR;
using Publishing.Core.Commands;
using Publishing.Core.DTOs;
using Publishing.Core.Interfaces;
using Publishing.Services;

namespace Publishing.AppLayer.Handlers;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IOrderRepository _repo;
    private readonly IOrderEventsPublisher _events;

    public UpdateOrderHandler(IOrderRepository repo, IOrderEventsPublisher events)
    {
        _repo = repo;
        _events = events;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        await _repo.UpdateStatusAsync(request.Id, request.Status.ToString());
        var personId = await _repo.GetPersonIdAsync(request.Id);
        if (personId is not null)
        {
            var dto = new OrderDto { PersonId = personId, Status = request.Status };
            _events.PublishOrderUpdated(dto);
        }
        return Unit.Value;
    }
}
