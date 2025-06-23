namespace Publishing.Services
{
    public interface IErrorHandler
    {
        void Handle(System.Exception ex);
    }
}
