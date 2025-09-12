import { ApolloClient, InMemoryCache, createHttpLink, from } from '@apollo/client/core';
import { onError } from '@apollo/client/link/error';
import { AuthLink } from './auth.link';

export function createApolloClient(authLink: AuthLink) {
  const errorLink = onError(({ graphQLErrors, networkError }) => {
    if (graphQLErrors) {
      graphQLErrors.forEach(({ message, locations, path }) => {
        console.error(`GraphQL Error: ${message}`, locations, path);
      });
    }
    if (networkError) {
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
