using FluentValidation;

namespace RepoRanger.Application.Sources.Commands.DeleteSourceCommand;

internal sealed class DeleteSourceCommandValidator : AbstractValidator<DeleteSourceCommand>
{
    public DeleteSourceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}