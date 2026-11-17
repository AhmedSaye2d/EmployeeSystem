namespace Application.Service.Contracts.Logging
{
    public interface IAppLogger<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(Exception Ex, string message);
    }
}
