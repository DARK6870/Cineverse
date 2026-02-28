import { Movie, MovieFilters, MovieSort } from '../api/movie.graphql.types';

export interface MoviesState {
  movies: Movie[];
  totalRecords: number;
  first: number;
  pageSize: number;
  sort: MovieSort;
  filters: MovieFilters;
  searchTerm: string;
  genreFilterOptions: string[];
}

export const DEFAULT_MOVIE_SORT: MovieSort = {
  field: 'dateCreated',
  direction: 'DESC',
};

export const initialMoviesState: MoviesState = {
  movies: [],
  totalRecords: 0,
  first: 0,
  pageSize: 25,
  sort: DEFAULT_MOVIE_SORT,
  filters: {},
  searchTerm: '',
  genreFilterOptions: [],
};
