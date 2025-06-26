namespace Publishing.Core.DTOs;

using Publishing.Core.Domain;

public class UpdateOrderDto
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
}
