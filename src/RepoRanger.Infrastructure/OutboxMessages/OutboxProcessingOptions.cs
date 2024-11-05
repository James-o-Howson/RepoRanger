namespace RepoRanger.Infrastructure.OutboxMessages;

public sealed class OutboxProcessingOptions
{
    public static string SectionKey => "OutboxProcessingOptions";
    
    public required int BatchSize { get; init; } = 100;
}