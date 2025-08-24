import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection, inject } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideApollo } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import { InMemoryCache } from '@apollo/client/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { providePrimeNG } from 'primeng/config';
import MyPreset from '../mypreset';
import { errorInterceptor } from './common/interceptors/error.interceptor';
import {MessageService} from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: MyPreset,
        options: {
          darkModeSelector: false
        }
      }
    }),
    MessageService,
    CookieService,
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([errorInterceptor])), provideApollo(() => {
      const httpLink = inject(HttpLink);

      return {
        // TODO: add routing
        link: httpLink.create({
          uri: 'http://localhost:7404/api/graphql',
        }),
        cache: new InMemoryCache(),
      };
    }),
    provideHttpClient(withInterceptors([errorInterceptor])),provideApollo(() => {
      const httpLink = inject(HttpLink);

      return {
        link: httpLink.create({
          uri: 'http://localhost:7404/api/graphql',
        }),
        cache: new InMemoryCache(),
      };
    })
  ],
};
