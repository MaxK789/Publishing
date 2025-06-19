namespace Publishing.Core.Interfaces
{
    using Publishing.Core.DTOs;

    public interface IAuthService
    {
        void OpenConnection();
        void CloseConnection();
        UserDto? Authenticate(string email, string password);
        UserDto Register(string firstName, string lastName, string email, string status, string password);
    }
}
