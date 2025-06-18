namespace Publishing.Core.Interfaces
{
    public interface IPrinteryRepository
    {
        decimal GetPricePerPage();
        int GetPagesPerDay();
    }
}
