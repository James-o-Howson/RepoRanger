namespace RepoRanger.Domain.Sources;

public interface ISourceParserService
{
    Task<IEnumerable<Source>> ParseAsync();
}