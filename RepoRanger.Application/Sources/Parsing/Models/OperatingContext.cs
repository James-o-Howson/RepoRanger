namespace RepoRanger.Application.Sources.Parsing.Models;

public sealed class OperatingContext<TContext>(SourceContext sourceContext)
{
    public SourceContext SourceContext { get; } = sourceContext;
    public TContext CurrentContext { get; set; }
}