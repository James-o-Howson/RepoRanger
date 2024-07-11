using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace RepoRanger.Application.Common.Extensions;

public static class LoggerExtensions
{
    public static void LogObject(this ILogger logger, LogLevel logLevel, object obj, string? message = null)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        var jsonString = JsonSerializer.Serialize(obj, options);
        if (message != null)
        {
            logger.Log(logLevel, "{Message} - Object: {Object}", message, jsonString);
        }
        else
        {
            logger.Log(logLevel, "Object: {Object}", jsonString);
        }
    }
}