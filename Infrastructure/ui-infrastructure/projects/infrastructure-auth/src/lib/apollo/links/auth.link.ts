import { ApolloLink, Observable } from '@apollo/client/core';
import { inject, Injectable, Injector } from '@angular/core';
import { AuthenticationService } from '../../services/authentication/authentication.service';

interface ApolloContext {
  allowAnonymous?: boolean;
  headers?: Record<string, string>;
}

@Injectable({ providedIn: 'root' })
export class AuthLink {
  private injector = inject(Injector);

  create(): ApolloLink {
    return new ApolloLink((operation, forward) => {
      const context = operation.getContext() as ApolloContext;

      if (context?.allowAnonymous) {
        return forward(operation);
      }

      const authenticationService =
        this.injector.get(AuthenticationService);

      return new Observable(observer => {
        authenticationService
          .getOrGenerateAccessTokenAsync()
          .then(token => {
            if (token) {
              operation.setContext({
                headers: {
                  ...context?.headers,
                  Authorization: `Bearer ${token}`,
                },
              });
            }

            forward(operation).subscribe(observer);
          })
          .catch(error => observer.error(error));
      });
    });
  }
}
