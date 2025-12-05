import { MutationOptions, QueryOptions } from '@apollo/client';
import { gql } from 'apollo-angular';
import { CreateBookingRequestInput } from './booking.graphql.types';

export const getBookedSeatsQuery = (screeningId: string) : QueryOptions => ({
  query: gql`
 query getBookedSeats($screeningId: String!){
  bookedSeats (screeningId: $screeningId)
}
`,
  variables: {screeningId: screeningId}
});

export const createBookingMutation = (request: CreateBookingRequestInput) : MutationOptions => ({
  mutation: gql`
  mutation createBooking($request: CreateBookingRequestInput!){
  createBooking(request: $request)
}
`,
  variables: {
    request: request
  }
});
