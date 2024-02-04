using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions;
using RepoRanger.Application.Abstractions.Exceptions;
using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Application.Sources.Commands.DeleteSourceCommand;

public sealed record DeleteSourceCommand : IRequest
{
    public Guid Id { get; init; }
} 

internal sealed class DeleteSourceCommandHandler : IRequestHandler<DeleteSourceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSourceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSourceCommand request, CancellationToken cancellationToken)
    {
        var source = await _context.Sources
            .Include(s => s.Repositories)
            .ThenInclude(r => r.Branches)
            .ThenInclude(b => b.Projects)
            .ThenInclude(p => p.Dependencies)
            .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (source is null) throw new NotFoundException($"Delete Failed - Unable find Source with Id: {request.Id}.");
        
        source.Delete();
        _context.Sources.Remove(source);
        await _context.SaveChangesAsync(cancellationToken);
    }
}