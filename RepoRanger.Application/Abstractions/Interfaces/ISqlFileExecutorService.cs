namespace RepoRanger.Application.Abstractions.Interfaces;

public interface ISqlFileExecutorService
{
    IQueryable<TEntity> ExecuteEmbeddedResource<TEntity>(string resourceName)
        where TEntity : class;
}