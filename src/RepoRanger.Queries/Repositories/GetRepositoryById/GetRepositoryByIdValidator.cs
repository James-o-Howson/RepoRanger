using FluentValidation;

namespace RepoRanger.Queries.Repositories.GetRepositoryById;

internal sealed class GetRepositoryByIdValidator : AbstractValidator<GetRepositoryByIdQuery>
{
    public GetRepositoryByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}