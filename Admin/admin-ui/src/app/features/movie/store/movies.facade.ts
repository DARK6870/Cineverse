import { Injectable, inject } from '@angular/core';
import { MovieGraphqlService } from '../api/movie.graphql.service';
import { CreateMovieRequestInput, Movie, MoviePage, UpdateMovieRequestInput } from '../api/movie.graphql.types';
import { MoviesStore } from './movies.store';

@Injectable()
export class MoviesFacade {
  private store = inject(MoviesStore);
  private movieService = inject(MovieGraphqlService);

  readonly movies$ = this.store.movies$;
  readonly totalRecords$ = this.store.totalRecords$;
  readonly pageSize$ = this.store.pageSize$;
  readonly first$ = this.store.first$;

  loadPage(first: number, pageSize: number): void {
    this.store.loadPage({ first, pageSize });
  }

  getMovies(first: number, pageSize: number): void {
    this.store.loadPage({ first, pageSize });
  }

  getMoviesPage(first: number, pageSize: number): Promise<MoviePage> {
    return this.movieService.getMoviesPage(first, pageSize);
  }

  getMoviesByIds(ids: string[]): Promise<Movie[]> {
    return this.movieService.getMoviesByIds(ids);
  }

  getMovieById(id: string): Promise<Movie> {
    return this.movieService.getMovieById(id);
  }

  async createMovie(request: CreateMovieRequestInput): Promise<string> {
    const result = await this.movieService.createMovie(request);
    this.store.refresh();
    return result;
  }

  async updateMovie(request: UpdateMovieRequestInput): Promise<string> {
    const result = await this.movieService.updateMovie(request);
    this.store.refresh();
    return result;
  }

  async deleteMovie(id: string): Promise<string> {
    const service = this.movieService as unknown as { deleteMovie?: (movieId: string) => Promise<string> };
    if (!service.deleteMovie) {
      throw new Error('deleteMovie is not available on MovieGraphqlService.');
    }
    const result = await service.deleteMovie(id);
    this.store.refresh();
    return result;
  }

  refresh(): void {
    this.store.refresh();
  }
}
