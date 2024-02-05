using MediatR;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Sources.Common.Mapping;
using RepoRanger.Application.Sources.Common.Models;

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

        await _context.Sources.AddAsync(source, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return source.Id;
    }
}