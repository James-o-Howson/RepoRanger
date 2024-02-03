using Microsoft.Extensions.Hosting;
using RepoRanger.Application.Abstractions.Options;

namespace RepoRanger.Persistence;

internal sealed class ConnectionStringsOptions : IOptions
{
    public string SectionName => "ConnectionStrings";

    public string RepoRangerDatabase { get; set; } = string.Empty;

    public bool IsValid() => 
        !string.IsNullOrEmpty(RepoRangerDatabase);
}