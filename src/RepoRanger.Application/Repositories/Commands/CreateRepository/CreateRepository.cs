﻿using MediatR;
using RepoRanger.Application.Common.Interfaces.Persistence;

namespace RepoRanger.Application.Repositories.Commands.CreateRepository;

public sealed record CreateRepositoryCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string RemoteUrl { get; set; }
    public string BranchName { get; set; }
    public bool BranchIsDefault { get; set; } = true;
}

internal sealed class CreateRepositoryCommandHandler : IRequestHandler<CreateRepositoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateRepositoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Guid> Handle(CreateRepositoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}