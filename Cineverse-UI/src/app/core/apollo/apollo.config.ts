import { InMemoryCache, createHttpLink, from, ApolloLink, Observable } from '@apollo/client/core';
import { onError } from '@apollo/client/link/error';
import { AuthLink } from './auth.link';
import { ToastService } from '../services/toast.service';
import { LoadingService } from '../services/loading.service';
import { inject } from '@angular/core';
import { BlockActionsService } from '../services/block-actions.service';

export function createApolloClient(authLink: AuthLink) {
  const toastService = inject(ToastService);
  const loadingService = inject(LoadingService);
  const blockActionsService = inject(BlockActionsService);

  const errorLink = onError(({ graphQLErrors, networkError }) => {
    if (graphQLErrors) {
      graphQLErrors.forEach(({ message }) => {
        toastService.error(message);
        console.error(message);
      });
    } else if (networkError) {
      console.error('Network Error:', networkError);
    }
  });

  const loadingLink = new ApolloLink((operation, forward) => {
    const isQuery = operation.query.definitions.some(
      (def: any) =>
        def.kind === 'OperationDefinition' && def.operation === 'query'
    );

    if (isQuery){
      loadingService.show();
    }
    else {
      blockActionsService.block();
    }


    return new Observable(observer => {
      const sub = forward(operation).subscribe({
        next: result => observer.next(result),
        error: error => {
          if (isQuery){
            loadingService.hide();
          }
          else {
            blockActionsService.unblock();
          }
          observer.error(error);
        },
        complete: () => {
          if (isQuery){
            loadingService.hide();
          }
          else {
            blockActionsService.unblock();
          }
          observer.complete();
        },
      });

      return () => {
        sub.unsubscribe();
        if (isQuery){
          loadingService.hide();
        }
        else {
          blockActionsService.unblock();
        }
      };
    });
  });

  const httpLink = createHttpLink({
    uri: '/cineverse-api',
  });

  return {
    link: from([errorLink, loadingLink, authLink.create(), httpLink]),
    cache: new InMemoryCache(),
  };
}
