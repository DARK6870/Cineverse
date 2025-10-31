import { setContext } from '@apollo/client/link/context';
import { Injectable, Injector } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

interface ApolloContext {
  allowAnonymous?: boolean;
  headers?: Record<string, string>;
}

@Injectable({ providedIn: 'root' })
export class AuthLink {
  constructor(private injector: Injector) {}

  create() {
    return setContext(async (operation, context: ApolloContext) => {
      if (context.allowAnonymous) {
        return {
          headers: context.headers || {}
        };
      }

      const authenticationService = this.injector.get(AuthenticationService);
      const token = await authenticationService.getOrGenerateAccessTokenAsync();

      if (!token) {
        return {
          headers: context.headers || {}
        };
      }

      return {
        headers: {
          ...context.headers,
          Authorization: `Bearer ${token}`
        }
      };
    });
  }
}
