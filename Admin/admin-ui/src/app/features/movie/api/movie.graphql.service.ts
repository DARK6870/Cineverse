import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import { CreateMovieRequestInput, Movie, MoviePage, UpdateMovieRequestInput } from './movie.graphql.types';
import {
  createMovieMutation,
  getMovieByIdQuery,
  getMoviesByIdsQuery,
  getMoviesPageQuery,
  updateMovieMutation,
} from './movie.graphql';

@Injectable({ providedIn: 'root' })
export class MovieGraphqlService {
  private apollo = inject(Apollo);

  public getMoviesByIds(ids: string[]): Promise<Movie[]> {
    return firstValueFrom(
      this.apollo
        .query<{ movies: { items: Movie[] } }>({
          ...getMoviesByIdsQuery(ids),
        })
        .pipe(map((res) => res.data!.movies.items)),
    );
  }

  public getMovieById(id: string): Promise<Movie> {
    return firstValueFrom(
      this.apollo
        .query<{ movieById: Movie }>({
          ...getMovieByIdQuery(id),
        })
        .pipe(map((res) => res.data!.movieById)),
    );
  }

  public getMoviesPage(skip: number, take: number): Promise<MoviePage> {
    return firstValueFrom(
      this.apollo
        .query<{ movies: MoviePage }>({
          ...getMoviesPageQuery(skip, take),
        })
        .pipe(map((res) => res.data!.movies)),
    );
  }

  public createMovie(request: CreateMovieRequestInput): Promise<string> {
    return firstValueFrom(
      this.apollo
        .mutate<{ createMovie: string }>({
          ...createMovieMutation(request)
        })
        .pipe(map((res) => res.data!.createMovie)),
    );
  }

  public updateMovie(request: UpdateMovieRequestInput): Promise<string> {
    return firstValueFrom(
      this.apollo
        .mutate<{ updateMovie: string }>({
          ...updateMovieMutation(request)
        })
        .pipe(map((res) => res.data!.updateMovie)),
    );
  }
}
