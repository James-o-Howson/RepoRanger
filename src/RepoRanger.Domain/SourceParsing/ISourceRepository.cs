using RepoRanger.Domain.Entities;

namespace RepoRanger.Domain.SourceParsing;

public interface ISourceRepository
{
    Task<Source?> GetSourceAsync(string name, CancellationToken cancellationToken);
}