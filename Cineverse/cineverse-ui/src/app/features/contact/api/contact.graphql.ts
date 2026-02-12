import { gql } from 'apollo-angular';
import { CreateContactRequestInput } from './contact.graphql.types';
import {MutationOptions} from '@apollo/client';

export const createContactRequestMutation = (request: CreateContactRequestInput) : MutationOptions => ({
  mutation: gql`
  mutation createContactRequest($request: CreateContactRequestInput!){
  createContactRequest(request: $request)
}
`,
  variables: {
    request: request
  },
  context: {
    allowAnonymous: true
  }
});
