using FluentValidation;

namespace RepoRanger.Application.Commands.Sources.DeleteSourceCommand;

internal sealed class DeleteSourceCommandValidator : AbstractValidator<DeleteSourceCommand>
{
    public DeleteSourceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}