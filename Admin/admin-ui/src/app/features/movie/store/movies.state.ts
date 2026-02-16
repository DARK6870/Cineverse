import { Movie } from '../api/movie.graphql.types';

export interface MoviesState {
  movies: Movie[];
  totalRecords: number;
  pageSize: number;
  first: number;
}

export const initialMoviesState: MoviesState = {
  movies: [],
  totalRecords: 0,
  pageSize: 25,
  first: 0,
};
