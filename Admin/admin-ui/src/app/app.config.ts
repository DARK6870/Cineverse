import { ApplicationConfig, inject, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { providePrimeNG } from 'primeng/config';
import { errorInterceptor, loadingInterceptor, PrimeNgPreset } from '@cineverse/infrastructure-common';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor, AuthLink } from '@cineverse/infrastructure-auth';
import { provideNamedApollo } from 'apollo-angular';
import { createNamedApolloClients } from './apollo/apollo.config';
import { ConfirmationService } from 'primeng/api';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([loadingInterceptor, authInterceptor, errorInterceptor])),
    provideNamedApollo(() => createNamedApolloClients(inject(AuthLink))),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideAnimations(),
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
