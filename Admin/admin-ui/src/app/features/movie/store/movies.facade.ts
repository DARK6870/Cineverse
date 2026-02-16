import { inject, Injectable } from '@angular/core';
import { MovieGraphqlService } from '../api/movie.graphql.service';
import { CreateMovieRequestInput, Movie, UpdateMovieRequestInput } from '../api/movie.graphql.types';
import { MoviesStore } from './movies.store';

@Injectable({ providedIn: 'root' })
export class MoviesFacade {
  private store = inject(MoviesStore);
  private movieService = inject(MovieGraphqlService);

  readonly movies = this.store.movies;
  readonly totalRecords = this.store.totalRecords;
  readonly pageSize = this.store.pageSize;
  readonly first = this.store.first;
  readonly loading = this.store.loading;

  loadPage(first: number, pageSize: number): void {
    this.store.loadPage(first, pageSize);
  }

  getMovieById(id: string): Promise<Movie> {
    return this.movieService.getMovieById(id);
  }

  async createMovie(request: CreateMovieRequestInput): Promise<string> {
    return await this.movieService.createMovie(request);
  }

  async updateMovie(request: UpdateMovieRequestInput): Promise<string> {
    return await this.movieService.updateMovie(request);
  }

  async deleteMovie(id: string) {
    await this.movieService.deleteMovie(id);
  }

  refresh(): void {
    this.store.refresh();
  }
}
