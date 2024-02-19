namespace RepoRanger.Application.Common.Interfaces.Persistence;

public interface ISqlFileExecutorService
{
    IQueryable<TEntity> ExecuteEmbeddedResource<TEntity>(string resourceName)
        where TEntity : class;
}