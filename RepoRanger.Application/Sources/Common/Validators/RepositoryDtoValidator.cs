using FluentValidation;
using RepoRanger.Application.Sources.Common.Models;

namespace RepoRanger.Application.Sources.Common.Validators;

internal sealed class RepositoryDtoValidator : AbstractValidator<RepositoryDto>
{
    public RepositoryDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty();
        
        RuleFor(r => r.RemoteUrl)
            .NotEmpty();
        
        RuleFor(r => r.Branches)
            .Must(r => r.Any());

        RuleForEach(r => r.Branches)
            .SetValidator(new BranchDtoValidator());
    }
}