using MediatR;
using RepoRanger.Application.Common.Exceptions;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Repositories.Common;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Sources.Commands.UpdateSourceCommand;

public sealed record UpdateSourceCommand : IRequest<int>
{
    public int Id { get; init; }
    public string Location { get; init; } = string.Empty; 
    public IReadOnlyCollection<RepositoryDto> Repositories { get; init; } = null!;
}

internal sealed class UpdateSourceCommandHandler : IRequestHandler<UpdateSourceCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateSourceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateSourceCommand request, CancellationToken cancellationToken)
    {
        var source = await _context.Sources.FindAsync([request.Id], cancellationToken);

        if (source is null) throw new NotFoundException($"Cannot find Source for Id: {request.Id}");
        
        source.Update(request.Location, request.Repositories.ToEntities().ToList());
        
        await _context.SaveChangesAsync(cancellationToken);
        return source.Id;
    }
}