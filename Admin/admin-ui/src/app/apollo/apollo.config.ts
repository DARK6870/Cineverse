import { InMemoryCache, HttpLink, ApolloLink } from '@apollo/client/core';
import { AuthLink } from '@cineverse/infrastructure-auth';
import { NamedOptions } from 'apollo-angular';
import { createErrorLink, createLoadingLink } from '@cineverse/infrastructure-common';

export const APOLLO_CLIENTS = {
  CINEVERSE: 'cineverse',
  IDENTITY: 'identity',
} as const;

function createClientOptions(authLink: AuthLink, uri: string) {
  const httpLink = new HttpLink({ uri });
  return {
    link: ApolloLink.from([createErrorLink(), createLoadingLink(), authLink.create(), httpLink]),
    cache: new InMemoryCache(),
  };
}

export function createNamedApolloClients(authLink: AuthLink): NamedOptions {
  return {
    [APOLLO_CLIENTS.CINEVERSE]: createClientOptions(authLink, '/api/cineverse/graphql'),
    [APOLLO_CLIENTS.IDENTITY]: createClientOptions(authLink, '/api/identity/graphql'),
  };
}
