export * from './repositories.service';
import { RepositoriesService } from './repositories.service';
export * from './sources.service';
import { SourcesService } from './sources.service';
export const APIS = [RepositoriesService, SourcesService];
