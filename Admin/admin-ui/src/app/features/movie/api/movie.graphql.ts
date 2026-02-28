import { gql } from 'apollo-angular';
import { CreateMovieRequestInput, MovieFilters, MovieSort, UpdateMovieRequestInput } from './movie.graphql.types';

const escapeGraphqlString = (value: string): string => value.replace(/\\/g, '\\\\').replace(/"/g, '\\"');

const buildMoviesWhereClause = (filters: MovieFilters): string => {
  const conditions: string[] = [];

  if (Array.isArray(filters.genre)) {
    if (filters.genre.length > 0) {
      const values = filters.genre.map((genre) => `"${escapeGraphqlString(genre)}"`).join(', ');
      conditions.push(`genre: { in: [${values}] }`);
    }
  } else if (filters.genre) {
    conditions.push(`genre: { eq: "${escapeGraphqlString(filters.genre)}" }`);
  }

  if (Array.isArray(filters.isAvailable)) {
    if (filters.isAvailable.length === 1) {
      conditions.push(`isAvailable: { eq: ${filters.isAvailable[0]} }`);
    } else if (filters.isAvailable.length > 1) {
      conditions.push(`isAvailable: { in: [${filters.isAvailable.join(', ')}] }`);
    }
  } else if (typeof filters.isAvailable === 'boolean') {
    conditions.push(`isAvailable: { eq: ${filters.isAvailable} }`);
  }

  if (conditions.length === 0) {
    return '';
  }

  return `where: { ${conditions.join(', ')} }`;
};

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
  variables: { ids: ids },
  context: {
    allowAnonymous: true
  }
});

export const getMovieByIdQuery = (id: string) => ({
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
      isAvailable
      dateCreated
  }
}
`,
  variables: { id },
  context: {
    allowAnonymous: true
  }
})

export const getMoviesPageQuery = (
  skip: number,
  take: number,
  sort: MovieSort,
  filters: MovieFilters,
  searchTerm?: string,
) => ({
  query: gql`
  query getMoviesPage($skip: Int!, $take: Int!, $searchTerm: String) {
    movies(
      skip: $skip,
      take: $take,
      searchTerm: $searchTerm,
      order: { ${sort.field}: ${sort.direction} }
      ${buildMoviesWhereClause(filters)}
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
        isAvailable
        dateCreated
      }
      totalCount
    }
  }
`,
  variables: { skip, take, searchTerm },
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

export const deleteMovieMutation = (id: string) => ({
  mutation: gql`
  mutation deleteMovieMutation($id: String!) {
    deleteMovie(id: $id)
  }
  `,
  variables: { id }
});

export const getGenreDistinctFilterValuesQuery =  ({
  query: gql`
  query getGenreDistinctFilterValues {
    genreDistinctFilterValues
  }
`,
  context: {
    allowAnonymous: true
  }
});
