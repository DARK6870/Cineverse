import { gql } from 'apollo-angular';

export const GET_MOVIES_QUERY = gql`
query getMovies($ids: [String]!) {
  movies(
    take: 250,
    order: {dateCreated: DESC },
    where: { id: { in: $ids } }
    )
    {
    items {
      id
      title
      genre
      description
      posterUrl
      trailerUrl
      releaseDate
      duration
      dateCreated
    }
  }
}
`
