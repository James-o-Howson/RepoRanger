using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing;

public interface ISourceParser
{
    Task<IEnumerable<SourceContext>> ParseAsync();
}