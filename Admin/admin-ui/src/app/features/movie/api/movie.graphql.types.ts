export interface Movie {
  id: string;
  title: string;
  genre: string;
  description: string;
  posterUrl: string;
  trailerUrl: string;
  releaseDate: string;
  duration: string;
  isAvailable: boolean;
}

export interface MoviePage {
  items: Movie[];
  totalCount: number;
}

export interface CreateMovieRequestInput {
  title: string;
  description: string;
  posterUrl: string;
  trailerUrl: string;
  duration: number;
  releaseDate: string;
  genre: string;
  isAvailable: boolean;
}

export interface UpdateMovieRequestInput extends CreateMovieRequestInput {
  id: string;
}
