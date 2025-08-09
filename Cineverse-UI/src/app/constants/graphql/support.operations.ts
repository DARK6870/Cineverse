import { gql } from 'apollo-angular';

const CREATE_SUPPORT_TICKET_MUTATION = gql`
mutation createSupportTicket($request: CreateSupportTicketRequestInput!){
  createSupportTicket(request: $request)
}
`

export { CREATE_SUPPORT_TICKET_MUTATION };
