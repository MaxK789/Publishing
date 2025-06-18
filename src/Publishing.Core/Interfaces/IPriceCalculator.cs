namespace Publishing.Core.Interfaces
{
    public interface IPriceCalculator
    {
        decimal Calculate(int pages, int copies, decimal pricePerPage);
    }
}
