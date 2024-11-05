namespace RepoRanger.Data.Abstractions;

internal sealed class ConnectionStringOptions
{
    internal const string SectionKey = "ConnectionStrings";
    public required string RepoRangerDatabase { get; init; }
}