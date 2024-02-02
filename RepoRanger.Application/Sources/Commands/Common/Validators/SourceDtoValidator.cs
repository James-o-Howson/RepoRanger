using FluentValidation;
using RepoRanger.Application.Sources.Commands.Common.Models;

namespace RepoRanger.Application.Sources.Commands.Common.Validators;

internal sealed class SourceDtoValidator : AbstractValidator<SourceDto>
{
    public SourceDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty();

        RuleFor(s => s.Repositories)
            .Must(r => r.Any());

        RuleForEach(s => s.Repositories)
            .SetValidator(new RepositoryDtoValidator());
    }
}