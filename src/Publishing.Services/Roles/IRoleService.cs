namespace Publishing.Services
{
    public interface IRoleService
    {
        bool IsAdmin(string userType);
        bool IsContactPerson(string userType);
        bool CanViewStatistics(string userType);
    }
}
