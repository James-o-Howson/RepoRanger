using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Sources.Parsing;

public interface ISourceParser
{
    Task<IEnumerable<Source>> ParseAsync();
}