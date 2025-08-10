import { gql } from 'apollo-angular';

const CREATE_CONTACT = gql`
mutation createContactRequest($request: CreateContactRequestInput!){
  createContactRequest(request: $request)
}
`

export { CREATE_CONTACT };
