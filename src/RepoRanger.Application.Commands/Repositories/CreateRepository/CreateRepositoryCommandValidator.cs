using FluentValidation;

namespace RepoRanger.Application.Commands.Repositories.CreateRepository;

internal sealed class CreateRepositoryCommandValidator : AbstractValidator<CreateRepositoryCommand>
{
    public CreateRepositoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();
        
        RuleFor(c => c.RemoteUrl)
            .NotEmpty();
        
        RuleFor(c => c.BranchName)
            .NotEmpty();
        
        RuleFor(c => c.VersionControlSystemId)
            .NotEmpty();
    }
}