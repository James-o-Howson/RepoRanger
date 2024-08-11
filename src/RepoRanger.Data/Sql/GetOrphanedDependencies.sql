SELECT *
FROM Dependencies
WHERE Id NOT IN (SELECT DependenciesId FROM DependencyProject)