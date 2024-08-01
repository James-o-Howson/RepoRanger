using FluentValidation;

namespace RepoRanger.Application.Commands.VersionControlSystems.DeleteVersionControlSystemCommand;

internal sealed class DeleteVersionControlSystemCommandValidator : AbstractValidator<DeleteVersionControlSystemCommand>
{
    public DeleteVersionControlSystemCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}