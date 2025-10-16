import { gql } from 'apollo-angular';
import { QueryOptions } from '@apollo/client';

export const getActiveScreeningMovieIdsQuery = ({
  query: gql`
  query getScreenings($currentDate: LocalDate!) {
    screenings(
    take: 250,
    where: { date: { gt: $currentDate } },
    order: { date: ASC }
  )
  {
    totalCount
     items {
       movieId
      }
   }
  }
  `,
  variables: {
    currentDate: new Date().toISOString().split('T')[0]
  },
  context: {
    allowAnonymous: true
  }
});

export const getActiveScreeningsByMovieIdQuery = (id : string) : QueryOptions => ({
  query: gql`
query getScreenings($movieId: String!, $currentDate: LocalDate!) {
  screenings(take: 25, where: {
    movieId: { eq: $movieId },
    date: { gt: $currentDate }
    }) {
    totalCount
    items {
      id
      movieId
      hallId
      date
      startTime
      endTime
      ticketPrice
      dateCreated
    }
  }
}
`,
  variables: {
    movieId: id,
    currentDate: new Date().toISOString().split('T')[0]
  },
  context: {
    allowAnonymous: true
  }
});


export const getScreeningByIdQuery = (id: string) : QueryOptions => ({
  query: gql`
query getScreeningById($id: String!) {
  screeningById(id: $id) {
    id
    movieId
    hallId
    date
    startTime
    endTime
    ticketPrice
    dateCreated
  }
}
`,
  variables: {
    id: id
  },
  context: {
    allowAnonymous: true
  }
});
