export interface Movie {
  id: string;
  title: string;
  genre: string;
  description: string;
  posterUrl: string;
  trailerUrl: string;
  rating?: number;
  releaseDate?: string;
  duration?: string;
}
