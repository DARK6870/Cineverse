import { gql } from 'apollo-angular';

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
