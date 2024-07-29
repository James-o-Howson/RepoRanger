namespace RepoRanger.Domain.VersionControlSystems.Parsing;

public interface IVersionControlSystemParserService
{
    Task ParseAsync(CancellationToken cancellationToken);
}