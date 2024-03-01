using RepoRanger.Domain.Entities;

namespace RepoRanger.Domain.SourceParsing;

public interface ISourceParserService
{
    Task<IEnumerable<Source>> ParseAsync();
}