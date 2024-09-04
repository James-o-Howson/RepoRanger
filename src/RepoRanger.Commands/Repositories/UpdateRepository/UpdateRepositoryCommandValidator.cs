using FluentValidation;

namespace RepoRanger.Commands.Repositories.UpdateRepository;

internal sealed class UpdateRepositoryCommandValidator : AbstractValidator<UpdateRepositoryCommand>
{
    public UpdateRepositoryCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
        
        RuleFor(c => c.Name)
            .NotEmpty();
        
        RuleFor(c => c.RemoteUrl)
            .NotEmpty();
        
        RuleFor(c => c.BranchName)
            .NotEmpty();
    }
}