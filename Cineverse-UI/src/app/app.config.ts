import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection, inject } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideApollo } from 'apollo-angular';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

import { providePrimeNG } from 'primeng/config';
import MyPreset from '../mypreset';
import { CookieService } from 'ngx-cookie-service';
import { createApolloClient } from './core/apollo/apollo.config';
import { AuthLink } from './core/apollo/auth.link';
import { CLIPBOARD_OPTIONS, ClipboardButtonComponent, provideMarkdown } from 'ngx-markdown';
import { ConfirmationService } from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    provideMarkdown({
      clipboardOptions: {
        provide: CLIPBOARD_OPTIONS,
        useValue: {
          buttonComponent: ClipboardButtonComponent,
        },
      },
    }),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: MyPreset,
        options: {
          darkModeSelector: false
        }
      }
    }),
    CookieService,
    ConfirmationService,
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(),
    provideApollo(() => createApolloClient(inject(AuthLink)))
  ]
};
