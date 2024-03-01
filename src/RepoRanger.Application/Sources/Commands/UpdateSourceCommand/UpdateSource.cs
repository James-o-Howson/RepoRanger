﻿using MediatR;
using RepoRanger.Application.Common.Exceptions;
using RepoRanger.Application.Common.Interfaces.Persistence;

namespace RepoRanger.Application.Sources.Commands.UpdateSourceCommand;

public sealed record UpdateSourceCommand : IRequest<Guid>
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Location { get; init; } = string.Empty; 
}

internal sealed class UpdateSourceCommandHandler : IRequestHandler<UpdateSourceCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateSourceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateSourceCommand request, CancellationToken cancellationToken)
    {
        var source = await _context.Sources.FindAsync([request.Id], cancellationToken);

        if (source is null) throw new NotFoundException($"Cannot find Source for Id: {request.Id}");

        source.Location = request.Location;
        return source.Id;
    }
}