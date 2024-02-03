using MediatR;
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
            .FindAsync([request.Id], cancellationToken);

        if (source is null) throw new NotFoundException($"Delete Failed - Unable find Source with Id: {request.Id}.");

        _context.RemoveEntity(source);
        await _context.SaveChangesAsync(cancellationToken);
    }
}