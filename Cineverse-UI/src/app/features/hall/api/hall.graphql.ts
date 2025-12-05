import { QueryOptions } from '@apollo/client';
import { gql } from 'apollo-angular';

export const getHallByIdQuery = (id: string): QueryOptions => ({
  query: gql`
    query getHallById($id: String!) {
      hallById(id: $id) {
        id
        name
        seats {
          seatId
          row
          number
        }
      }
    }
  `,
  variables: { id: id },
});
