import { gql } from 'apollo-angular';
import { CreateHallRequestInput, HallSort, UpdateHallRequestInput } from './hall.graphql.types';

export const getHallsPageQuery = (skip: number, take: number, sort: HallSort) => ({
  query: gql`
    query getHallsPage($skip: Int!, $take: Int!) {
      halls(
        skip: $skip,
        take: $take,
        order: { ${sort.field}: ${sort.direction} }
      ) {
        items {
          id
          name
          dateCreated
          seats {
            seatId
            row
            number
          }
        }
        totalCount
      }
    }
  `,
  variables: { skip, take },
  context: {
    allowAnonymous: true
  }
});

export const getHallByIdQuery = (id: string) => ({
  query: gql`
    query getHallById($id: String!) {
      hallById(id: $id) {
        id
        name
        dateCreated
        seats {
          seatId
          row
          number
        }
      }
    }
  `,
  variables: { id },
  context: {
    allowAnonymous: true
  }
});

export const createHallMutation = (request: CreateHallRequestInput) => ({
  mutation: gql`
    mutation createHallMutation($request: CreateHallRequestInput!) {
      createHall(request: $request)
    }
  `,
  variables: { request }
});

export const updateHallMutation = (request: UpdateHallRequestInput) => ({
  mutation: gql`
    mutation updateHallMutation($request: UpdateHallRequestInput!) {
      updateHall(request: $request)
    }
  `,
  variables: { request }
});

export const deleteHallMutation = (id: string) => ({
  mutation: gql`
    mutation deleteHallMutation($id: String!) {
      deleteHall(id: $id)
    }
  `,
  variables: { id }
});
