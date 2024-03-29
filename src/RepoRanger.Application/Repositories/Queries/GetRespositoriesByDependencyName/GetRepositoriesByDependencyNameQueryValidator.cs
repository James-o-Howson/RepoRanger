﻿using FluentValidation;

namespace RepoRanger.Application.Repositories.Queries.GetRespositoriesByDependencyName;

internal sealed class GetRepositoriesByDependencyNameQueryValidator : AbstractValidator<GetRepositoriesByDependencyNameQuery>
{
    public GetRepositoriesByDependencyNameQueryValidator()
    {
        RuleFor(q => q.DependencyName)
            .NotEmpty();
    }
}