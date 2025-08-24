import { gql } from 'apollo-angular';

const GET_SCREENINGS_QUERY = gql`
query getScreenings($currentDate: LocalDate!) {
  screenings(
  take: 10,
  where: {
    date: { gt: $currentDate }
    },
  order: { date: ASC }
  )
  {
    totalCount
    items {
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
`//TODO: retrieve only movie id

export { GET_SCREENINGS_QUERY };
