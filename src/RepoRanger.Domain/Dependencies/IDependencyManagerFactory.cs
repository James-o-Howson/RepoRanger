namespace RepoRanger.Domain.Dependencies;

public interface IDependencyManagerFactory
{
    Task<IDependencyManager> CreateAsync(CancellationToken cancellationToken = default);
}