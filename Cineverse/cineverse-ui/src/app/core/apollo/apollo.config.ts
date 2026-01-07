import { InMemoryCache, createHttpLink, from } from '@apollo/client/core';
import { createErrorLink, createLoadingLink } from 'infrastructure-common';
import { AuthLink } from '@cineverse/infrastructure-auth';

export function createApolloClient(authLink: AuthLink) {
  const httpLink = createHttpLink({
    uri: '/cineverse-api',
  });

  return {
    link: from([createErrorLink(), createLoadingLink(), authLink.create(), httpLink]),
    cache: new InMemoryCache(),
  };
}
