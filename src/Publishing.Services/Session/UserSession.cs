namespace Publishing.Services
{
    public class UserSession : IUserSession
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
    }
}
