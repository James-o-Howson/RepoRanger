import { ApplicationConfig, ErrorHandler } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { DATETIME_CONFIG_PROVIDERS } from './core/config/datetime-config';
import { provideHttpClient } from '@angular/common/http';
import { BASE_PATH } from './generated';
import { ErrorHandlerService } from './core/error-handler.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    provideAnimationsAsync(), 
    ...DATETIME_CONFIG_PROVIDERS,
    provideHttpClient(),
    { provide: BASE_PATH, useValue: "https://localhost:7263" },
    { provide: ErrorHandler, useClass: ErrorHandlerService }
  ]
};
