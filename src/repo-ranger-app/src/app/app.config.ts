import { ApplicationConfig, LOCALE_ID } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { API_BASE_URL, DependencyInstancesClient, ProjectsClient, RepositoriesClient, SourcesClient } from './api-client';
import { DatePipe } from '@angular/common';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(),
    DependencyInstancesClient,
    ProjectsClient,
    RepositoriesClient,
    SourcesClient,
    DatePipe,
    { provide: API_BASE_URL, useValue: "https://localhost:7263" },
    { provide: LOCALE_ID, useValue: 'en-AU' }
  ]
};
