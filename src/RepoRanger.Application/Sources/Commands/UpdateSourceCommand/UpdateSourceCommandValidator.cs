using FluentValidation;
using RepoRanger.Application.Repositories.Common;

namespace RepoRanger.Application.Sources.Commands.UpdateSourceCommand;

internal sealed class UpdateSourceCommandValidator : AbstractValidator<UpdateSourceCommand>
{
    public UpdateSourceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Location)
            .NotEmpty();

        RuleForEach(c => c.Repositories)
            .SetValidator(new RepositoryDtoValidator());
    }
}