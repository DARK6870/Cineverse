import { gql } from 'apollo-angular';
import { QueryOptions } from '@apollo/client';

export const getMoviesByIdsQuery = (ids: string[]) : QueryOptions => ({
  query: gql`
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
`,
  variables: {ids : ids},
  context: {
    allowAnonymous: true
  }
});

export const getComingSoonMovies = ({
  query: gql`
  query getMovies($releaseDate: LocalDate!) {
  movies(
    take: 250,
    order: {dateCreated: DESC },
    where: { releaseDate: { gt: $releaseDate } }
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
`,
  variables: {releaseDate : new Date(new Date().setDate(new Date().getDate() + 7)).toISOString().split('T')[0]},
  context: {
    allowAnonymous: true
  }
})

export const getMovieByIdQuery = (id : string) : QueryOptions => ({
  query: gql`
  query getMovieById($id: String!){
  movieById(id: $id){
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
`,
  variables: {id : id},
  context: {
    allowAnonymous: true
  }
})
