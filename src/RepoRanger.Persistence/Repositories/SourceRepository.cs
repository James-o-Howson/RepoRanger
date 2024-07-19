using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.SourceParsing;

namespace RepoRanger.Persistence.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly IApplicationDbContext _dbContext;

    public SourceRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Source?> GetSourceAsync(string name, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        return await _dbContext.Sources
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }
}