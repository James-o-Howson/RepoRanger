import { ApplicationConfig, ErrorHandler, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { API_BASE_URL, DependencyInstancesClient, ProjectsClient, RepositoriesClient, SourcesClient } from './api-client';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(),
    DependencyInstancesClient,
    ProjectsClient,
    RepositoriesClient,
    SourcesClient,
    { provide: API_BASE_URL, useValue: "https://localhost:7263" }
  ]
};
