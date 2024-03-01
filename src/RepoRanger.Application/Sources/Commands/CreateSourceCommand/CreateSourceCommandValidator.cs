using FluentValidation;
using RepoRanger.Application.Repositories.Common;

namespace RepoRanger.Application.Sources.Commands.CreateSourceCommand;

internal sealed class CreateSourceCommandValidator : AbstractValidator<CreateSourceCommand>
{
    public CreateSourceCommandValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty();
        
        RuleFor(s => s.Location)
            .NotEmpty();
        
        RuleForEach(c => c.Repositories)
            .SetValidator(new RepositoryDtoValidator());
    }
}