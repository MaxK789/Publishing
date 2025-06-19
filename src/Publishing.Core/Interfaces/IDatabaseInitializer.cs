namespace Publishing.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
