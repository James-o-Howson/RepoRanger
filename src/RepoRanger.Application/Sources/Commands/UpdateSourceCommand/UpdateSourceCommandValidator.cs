using FluentValidation;

namespace RepoRanger.Application.Sources.Commands.UpdateSourceCommand;

internal sealed class UpdateSourceCommandValidator : AbstractValidator<UpdateSourceCommand>
{
    public UpdateSourceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Location)
            .NotEmpty();
    }
}