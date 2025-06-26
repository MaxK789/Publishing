using MediatR;
using Publishing.Core.Domain;

namespace Publishing.Core.Commands;

public record UpdateOrderCommand(int Id, OrderStatus Status) : IRequest<Unit>;
