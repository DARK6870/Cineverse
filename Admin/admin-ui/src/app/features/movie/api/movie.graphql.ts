import { gql } from 'apollo-angular';
import { CreateMovieRequestInput, UpdateMovieRequestInput } from './movie.graphql.types';

export const getMoviesByIdsQuery = (ids: string[]) => ({
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

export const getMovieByIdQuery = (id : string) => ({
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
  variables: { id },
  context: {
    allowAnonymous: true
  }
})

export const getMoviesPageQuery = (skip: number, take: number) => ({
  query: gql`
  query getMoviesPage($skip: Int!, $take: Int!) {
    movies(
      skip: $skip,
      take: $take,
      order: { dateCreated: DESC }
    ) {
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
      totalCount
    }
  }
`,
  variables: { skip, take },
  context: {
    allowAnonymous: true
  }
});

export const createMovieMutation = (request: CreateMovieRequestInput) => ({
  mutation: gql`
  mutation createMovieMutation($request: CreateMovieRequestInput!) {
    createMovie(request: $request)
  }
`,
  variables: { request }
});

export const updateMovieMutation = (request: UpdateMovieRequestInput) => ({
  mutation: gql`
  mutation updateMovieMutation($request: UpdateMovieRequestInput!) {
    updateMovie(request: $request)
  }
`,
  variables: { request }
});
