using RepoRanger.Application.Sources.Queries.GetSourceDetails;

namespace RepoRanger.Application.Abstractions.Interfaces;

public interface IRepoRangerService
{
    Task<SourceDetailsVm?> GetSourceDetailsAsync();
}