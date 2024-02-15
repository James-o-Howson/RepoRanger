using FluentValidation;

namespace RepoRanger.Application.Abstractions.Pagination;

internal sealed class PaginatedRequestValidator<TQuery> : AbstractValidator<PaginatedRequest<TQuery>>
{
    public PaginatedRequestValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThan(0);
        
        RuleFor(q => q.PageSize)
            .GreaterThan(0);
    }
}