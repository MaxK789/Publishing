namespace Publishing.Core.Interfaces
{
    public interface IDiscountPolicy
    {
        decimal ApplyDiscount(int pages, int copies, decimal basePrice);
    }
}
