namespace Publishing.Services
{
    public interface IUserSession
    {
        string UserId { get; set; }
        string UserName { get; set; }
        string UserType { get; set; }
    }
}
