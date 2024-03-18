using FluentValidation;

namespace RepoRanger.Application.Repositories.Queries.GetRepositoryById;

internal sealed class GetRepositoryByIdValidator : AbstractValidator<GetRepositoryByIdQuery>
{
    public GetRepositoryByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}