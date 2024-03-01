using MediatR;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Dependencies.Commands.CreateDependency;

public sealed record CreateDependencyCommand : IRequest
{
    public string Name { get; init; } = string.Empty;
}

internal sealed class CreateDependencyCommandHandler : IRequestHandler<CreateDependencyCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateDependencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateDependencyCommand request, CancellationToken cancellationToken)
    {
        await _context.Dependencies.AddAsync(Dependency.CreateInstance(request.Name), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}