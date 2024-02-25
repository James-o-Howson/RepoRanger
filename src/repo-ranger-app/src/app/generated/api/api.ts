export * from './dependency-instances.service';
import { DependencyInstancesService } from './dependency-instances.service';
export * from './projects.service';
import { ProjectsService } from './projects.service';
export * from './repositories.service';
import { RepositoriesService } from './repositories.service';
export * from './sources.service';
import { SourcesService } from './sources.service';
export const APIS = [DependencyInstancesService, ProjectsService, RepositoriesService, SourcesService];
