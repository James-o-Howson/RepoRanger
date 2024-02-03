namespace RepoRanger.Application.Sources.Parsing.Models;

public interface ISourceContext
{
}

public sealed class SourceContext : ISourceContext
{
    public string Name { get; init; } = string.Empty;
    public IEnumerable<RepositoryContext> RepositoryContexts { get; set;  } = [];
    
}