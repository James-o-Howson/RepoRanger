using FluentValidation;
using RepoRanger.Application.Sources.Common.Validators;

namespace RepoRanger.Application.Sources.Commands.CreateSourceCommand;

internal sealed class CreateSourceCommandValidator : AbstractValidator<CreateSourceCommand>
{
    public CreateSourceCommandValidator()
    {
        Include(new SourceDtoValidator());
    }
}