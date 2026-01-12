import { ApplicationConfig, inject, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { providePrimeNG } from 'primeng/config';
import { errorInterceptor, loadingInterceptor, PrimeNgPreset } from '@cineverse/infrastructure-common';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor, AuthLink } from '@cineverse/infrastructure-auth';
import { provideApollo } from 'apollo-angular';
import { createApolloClient } from './apollo/apollo.config';
import { ConfirmationService } from 'primeng/api';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([loadingInterceptor, authInterceptor, errorInterceptor])),
    provideApollo(() => createApolloClient(inject(AuthLink))),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: PrimeNgPreset,
        options: {
          darkModeSelector: false
        }
      }
    }),
    ConfirmationService
  ],
};
