import { UpdatePersonalInformationRequestInput } from './user.graphql.types';
import { MutationOptions } from '@apollo/client';
import { gql } from 'apollo-angular';

export const updatePersonalInformationMutation = (request: UpdatePersonalInformationRequestInput) : MutationOptions => ({
  mutation: gql`
  mutation updatePersonalInformation($request: UpdatePersonalInformationRequestInput!){
  updatePersonalInformation(request: $request)
}
`,
  variables: {
    request: request
  }
})
