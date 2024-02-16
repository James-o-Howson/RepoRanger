using FluentValidation;
using RepoRanger.Application.Repositories.Common;

namespace RepoRanger.Application.Sources.Common;

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