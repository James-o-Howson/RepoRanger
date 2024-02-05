using FluentValidation;
using RepoRanger.Application.Sources.Common.Models;

namespace RepoRanger.Application.Sources.Common.Validators;

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