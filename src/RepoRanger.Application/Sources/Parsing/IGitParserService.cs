using RepoRanger.Domain.Entities;
using RepoRanger.Domain.Sources;

namespace RepoRanger.Application.Sources.Parsing;

public interface IGitParserService
{
    Task<IEnumerable<Source>> ParseAsync();
}