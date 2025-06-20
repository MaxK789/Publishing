using Publishing.Core.Interfaces;

namespace Publishing.Core.Services
{
    public class StandardDiscountPolicy : IDiscountPolicy
    {
        public decimal ApplyDiscount(int pages, int copies, decimal basePrice)
        {
            return basePrice;
        }
    }
}
