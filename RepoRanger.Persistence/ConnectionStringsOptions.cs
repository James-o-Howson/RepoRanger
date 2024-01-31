using Microsoft.Extensions.Hosting;
using RepoRanger.Application.Options;

namespace RepoRanger.Persistence;

internal sealed class ConnectionStringsOptions : IOptions
{
    public string SectionName => "ConnectionStrings";

    public string RepoRangerDatabase { get; set; }

    public bool IsValid(IHostEnvironment environment) => 
        !string.IsNullOrEmpty(RepoRangerDatabase);
}