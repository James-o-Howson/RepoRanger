namespace RepoRanger.Domain.SourceParsing;

public interface ISourceParserService
{
    Task ParseAsync(CancellationToken cancellationToken);
}