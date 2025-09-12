import { gql } from 'apollo-angular';

export const CREATE_CONTACT = gql`
mutation createContactRequest($request: CreateContactRequestInput!){
  createContactRequest(request: $request)
}
`
