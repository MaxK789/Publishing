using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IOrderValidator
    {
        void Validate(CreateOrderDto dto);
    }
}
