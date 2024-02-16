using FluentValidation;
using RepoRanger.Application.Branches;

namespace RepoRanger.Application.Repositories.Common;

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