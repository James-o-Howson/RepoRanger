using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Repositories.Common;
using RepoRanger.Application.Sources.Common;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Sources.Commands.CreateSourceCommand;

public sealed record CreateSourceCommand(string Name, IEnumerable<RepositoryDto> Repositories) 
    : SourceDto(Name, Repositories), IRequest<Guid>;

internal sealed class CreateSourceCommandHandler : IRequestHandler<CreateSourceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSourceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSourceCommand request, CancellationToken cancellationToken)
    {
        var source = request.ToEntity();
        
        await CreateDependencies(
            source.DependencyInstances.Select(di => di.DependencyName).ToHashSet(), 
            cancellationToken);
        
        await _context.Sources.AddAsync(source, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return source.Id;
    }

    private async Task CreateDependencies(IEnumerable<string> dependencies, CancellationToken cancellationToken)
    {
        foreach (var dependency in dependencies)
        {
            var entity = new Dependency(dependency);
            await _context.Dependencies.AddIfNotExistsAsync(entity, cancellationToken: cancellationToken);
        }
    }
}