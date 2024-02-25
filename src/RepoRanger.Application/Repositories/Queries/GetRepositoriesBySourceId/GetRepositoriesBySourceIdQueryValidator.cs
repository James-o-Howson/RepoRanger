using FluentValidation;

namespace RepoRanger.Application.Repositories.Queries.GetRepositoriesBySourceId;

internal sealed class GetRepositoriesBySourceIdQueryValidator : AbstractValidator<GetRepositoriesBySourceIdQuery>
{
    public GetRepositoriesBySourceIdQueryValidator()
    {
        RuleFor(q => q.SourceId)
            .NotEmpty();
    }
}