using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Sources.Parsing;

public interface ISourceParser
{
    Task<IEnumerable<Source>> ParseAsync();
}