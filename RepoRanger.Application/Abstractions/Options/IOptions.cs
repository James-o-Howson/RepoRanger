using Microsoft.Extensions.Hosting;

namespace RepoRanger.Application.Abstractions.Options;

public interface IOptions
{
    string SectionName { get; }
    bool IsValid(IHostEnvironment environment);
}