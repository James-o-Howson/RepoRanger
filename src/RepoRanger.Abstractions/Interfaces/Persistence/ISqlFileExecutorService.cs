namespace RepoRanger.Abstractions.Interfaces.Persistence;

public interface ISqlFileExecutorService
{
    IQueryable<TEntity> ExecuteEmbeddedResource<TEntity>(string resourceName)
        where TEntity : class;
}