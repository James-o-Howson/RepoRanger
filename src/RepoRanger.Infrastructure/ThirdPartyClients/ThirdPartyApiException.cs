namespace RepoRanger.Infrastructure.ThirdPartyClients;

public class ThirdPartyApiException : Exception
{
    public ThirdPartyApiException(string? message)
        : base(message)
    {
    }

    public ThirdPartyApiException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}