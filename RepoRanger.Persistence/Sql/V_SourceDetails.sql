CREATE VIEW SourceDetail
AS
SELECT
    S.Id,
    S.Name,
    S.Created as ParseTime,
    B.Id as DefaultBranchId,
    B.Name as DefaultBranchName,
    COUNT(DISTINCT P.Id) as ProjectsCount,
    COUNT(DISTINCT D.Id) as DependenciesCount
FROM main.Sources S
JOIN main.Repositories R on S.Id = R.SourceId
JOIN main.Branches B on R.Id = B.RepositoryId
JOIN main.BranchProject BP on B.Id = BP.BranchesId
JOIN main.Projects P on BP.ProjectsId = P.Id
JOIN main.DependencyProject DP on P.Id = DP.ProjectsId
JOIN main.Dependencies D on DP.DependenciesId = D.Id
WHERE B.IsDefault = true
GROUP BY S.Id;