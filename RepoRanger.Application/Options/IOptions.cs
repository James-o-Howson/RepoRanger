using Microsoft.Extensions.Hosting;

namespace RepoRanger.Application.Options;

public interface IOptions
{
    string SectionName { get; }
    bool IsValid(IHostEnvironment environment);
}