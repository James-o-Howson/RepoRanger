using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing.Common;

namespace RepoRanger.Infrastructure.SourceParsing;

internal interface ISourceParserResultHandler
{
    Task HandleAsync(IEnumerable<ParsedSourceResult> results, CancellationToken cancellationToken);
}

internal sealed class SourceParserResultHandler : ISourceParserResultHandler
{
    private readonly IApplicationDbContext _dbContext;

    public SourceParserResultHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(IEnumerable<ParsedSourceResult> results, CancellationToken cancellationToken)
    {
        foreach (var result in results)
        {
            await CreateOrUpdateAsync(result, cancellationToken);
        }
    }
    
    private async Task CreateOrUpdateAsync(ParsedSourceResult result, CancellationToken cancellationToken)
    {
        if (result.IsNewSource)
        {
            var source = result.Parsed;
            
            await CreateOrUpdateDependencies(source.Dependencies, cancellationToken);
            await _dbContext.Sources.AddAsync(source, cancellationToken);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(result.Existing);
             await CreateOrUpdateDependencies(result.Existing.Dependencies, cancellationToken);
            result.Existing.Update(result.Existing.Location, result.Existing.Repositories.ToList());
            
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task CreateOrUpdateDependencies(IEnumerable<string> dependencies, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Dependencies.ToListAsync(cancellationToken);
        
        var newDependencies = Dependency.Create(dependencies).Except(existing);
        await _dbContext.Dependencies.AddRangeAsync(newDependencies, cancellationToken);
    }
}