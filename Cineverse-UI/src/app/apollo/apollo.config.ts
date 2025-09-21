import { InMemoryCache, createHttpLink, from } from '@apollo/client/core';
import { onError } from '@apollo/client/link/error';
import { AuthLink } from './auth.link';
import { ToastService } from '../services/toast/toast.service';
import { inject } from '@angular/core';

export function createApolloClient(authLink: AuthLink) {
  const toastService = inject(ToastService);

  const errorLink = onError(({ graphQLErrors, networkError }) => {
    if (graphQLErrors) {
      graphQLErrors.forEach(({ message }) => {
        toastService.error(message);
        console.error(message);
      });
    }
    else if (networkError) {
      console.error('Network Error:', networkError);
    }
  });

  const httpLink = createHttpLink({
    uri: 'cineverse-api'
  });

  return ({
    link: from([errorLink, authLink.create(), httpLink]),
    cache: new InMemoryCache(),
  });
}
