using FluentValidation;

namespace RepoRanger.Abstractions.Pagination;

public sealed class PaginatedRequestValidator<TQuery> : AbstractValidator<PaginatedRequest<TQuery>>
{
    public PaginatedRequestValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThan(0);
        
        RuleFor(q => q.PageSize)
            .GreaterThan(0);
    }
}