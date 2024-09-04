using FluentValidation;

namespace RepoRanger.Commands.Repositories.DeleteRepository;

internal sealed class DeleteRepositoryCommandValidator : AbstractValidator<DeleteRepositoryCommand>
{
    public DeleteRepositoryCommandValidator() =>
        RuleFor(c => c.RepositoryId).NotEmpty();
}