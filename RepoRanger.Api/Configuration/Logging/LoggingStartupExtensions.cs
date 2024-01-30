using Serilog;

namespace RepoRanger.Api.Configuration.Logging;

internal static class LoggingStartupExtensions
{
    public static void UseSerilog(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        SerilogHostBuilderExtensions.UseSerilog(hostBuilder);
    }
}