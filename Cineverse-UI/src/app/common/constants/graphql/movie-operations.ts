import { gql } from 'apollo-angular';

const GET_MOVIES = gql`
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

export { GET_MOVIES };
