namespace RepoRanger.Api.Configuration.ExceptionHandlers;

internal static class ExceptionHandlerStartupExtensions
{
    public static void AddExceptionHandlerServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}