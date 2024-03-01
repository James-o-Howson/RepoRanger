using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Repositories.Common;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Sources.Commands.CreateSourceCommand;

public sealed record CreateSourceCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public IReadOnlyCollection<RepositoryDto> Repositories { get; init; } = null!;
}

internal sealed class CreateSourceCommandHandler : IRequestHandler<CreateSourceCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSourceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSourceCommand request, CancellationToken cancellationToken)
    {
        var repositories = request.Repositories.ToEntities();
        var source = Source.Create(request.Name, request.Location, repositories);
        
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
            var entity = Dependency.CreateInstance(dependency);
            await _context.Dependencies.AddIfNotExistsAsync(entity, cancellationToken: cancellationToken);
        }
    }
}