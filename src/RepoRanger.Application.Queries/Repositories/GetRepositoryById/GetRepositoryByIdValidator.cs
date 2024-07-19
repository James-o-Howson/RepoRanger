using FluentValidation;

namespace RepoRanger.Application.Queries.Repositories.GetRepositoryById;

internal sealed class GetRepositoryByIdValidator : AbstractValidator<GetRepositoryByIdQuery>
{
    public GetRepositoryByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}