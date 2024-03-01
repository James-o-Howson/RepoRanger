using FluentValidation;
using RepoRanger.Application.Projects.Common;

namespace RepoRanger.Application.Repositories.Common;

internal sealed class RepositoryDtoValidator : AbstractValidator<RepositoryDto>
{
    public RepositoryDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty();
        
        RuleFor(r => r.RemoteUrl)
            .NotEmpty();

        RuleFor(r => r.Branch)
            .NotEmpty();
        
        RuleFor(b => b.Projects)
            .Must(r => r.Any());

        RuleForEach(b => b.Projects)
            .SetValidator(new ProjectDtoValidator());
    }
}