using FluentValidation;
using RepoRanger.Application.Sources.Common;

namespace RepoRanger.Application.Sources.Commands.CreateSourceCommand;

internal sealed class CreateSourceCommandValidator : AbstractValidator<CreateSourceCommand>
{
    public CreateSourceCommandValidator()
    {
        Include(new SourceDtoValidator());
    }
}