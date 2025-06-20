using MediatR;
using Publishing.Core.Domain;

namespace Publishing.AppLayer.Commands
{
    public record CreateOrderCommand(
        string Type,
        string Name,
        int Pages,
        int Tirage,
        string PersonId,
        string Printery) : IRequest<Order>;
}
