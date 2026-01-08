import { InMemoryCache, HttpLink, ApolloLink } from '@apollo/client/core';
import { AuthLink } from '@cineverse/infrastructure-auth';
import { createErrorLink, createLoadingLink } from '@cineverse/infrastructure-common';

export function createApolloClient(authLink: AuthLink) {
  const httpLink = new HttpLink({
    uri: '/api/identity/graphql',
  });

  return {
    link: ApolloLink.from([createErrorLink(), createLoadingLink(), authLink.create(), httpLink]),
    cache: new InMemoryCache(),
  };
}
