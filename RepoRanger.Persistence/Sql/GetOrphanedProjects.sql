SELECT *
FROM Projects
WHERE Id NOT IN (SELECT ProjectsId FROM DependencyProject)
AND Id NOT IN (SELECT ProjectsId FROM BranchProject)
