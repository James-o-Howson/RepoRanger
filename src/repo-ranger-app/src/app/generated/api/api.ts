export * from './dependencies.service';
import { DependenciesService } from './dependencies.service';
export * from './projects.service';
import { ProjectsService } from './projects.service';
export * from './repositories.service';
import { RepositoriesService } from './repositories.service';
export * from './sources.service';
import { SourcesService } from './sources.service';
export const APIS = [DependenciesService, ProjectsService, RepositoriesService, SourcesService];
