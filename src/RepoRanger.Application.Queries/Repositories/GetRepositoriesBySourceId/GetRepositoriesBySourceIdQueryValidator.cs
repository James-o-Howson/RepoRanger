using FluentValidation;

namespace RepoRanger.Application.Queries.Repositories.GetRepositoriesBySourceId;

internal sealed class GetRepositoriesBySourceIdQueryValidator : AbstractValidator<GetRepositoriesBySourceIdQuery>
{
    public GetRepositoriesBySourceIdQueryValidator()
    {
        RuleFor(q => q.SourceId)
            .NotEmpty();
    }
}