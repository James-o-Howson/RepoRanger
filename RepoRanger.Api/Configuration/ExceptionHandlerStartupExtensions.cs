using RepoRanger.Api.Middleware;

namespace RepoRanger.Api.Configuration;

internal static class ExceptionHandlerStartupExtensions
{
    public static void AddExceptionHandlerServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}