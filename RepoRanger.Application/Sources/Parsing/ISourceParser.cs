using RepoRanger.Application.Sources.Parsing.Models;

namespace RepoRanger.Application.Sources.Parsing;

public interface ISourceParser
{
    IEnumerable<SourceContext> Parse();
}