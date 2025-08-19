import { gql } from 'apollo-angular';

const GET_MOVIES = gql`
query getMovies {
  movies(take: 10, order: { dateCreated: DESC }) {
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
