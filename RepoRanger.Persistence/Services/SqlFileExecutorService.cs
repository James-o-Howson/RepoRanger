using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Persistence.Services;

internal sealed class SqlFileExecutorService : ISqlFileExecutorService
{
    private readonly IApplicationDbContext _context;

    public SqlFileExecutorService(IApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> ExecuteEmbeddedResource<TEntity>(string resourceName)
        where TEntity : class
    {
        ArgumentException.ThrowIfNullOrEmpty(resourceName);

        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream is null) throw new ArgumentException($"Invalid sql file resource name: {resourceName}", nameof(resourceName));
        using var streamReader = new StreamReader(stream);
        if (streamReader is null) throw new ArgumentException($"Unable to read resource from file: {resourceName}", nameof(resourceName));
        
        var sql = FormattableStringFactory.Create(streamReader.ReadToEnd());
        
        return _context.Set<TEntity>().FromSql(sql);
    }
}