import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { providePrimeNG } from 'primeng/config';
import { errorInterceptor, loadingInterceptor, PrimeNgPreset } from '@cineverse/infrastructure-common';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from '@cineverse/infrastructure-auth';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([errorInterceptor, loadingInterceptor, authInterceptor])),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    providePrimeNG({
      theme: {
        preset: PrimeNgPreset,
        options: {
          darkModeSelector: false
        }
      }
    })
  ]
};
