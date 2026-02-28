import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import { CreateMovieRequestInput, Movie, MovieFilters, MoviePage, MovieSort, UpdateMovieRequestInput } from './movie.graphql.types';
import {
  createMovieMutation,
  deleteMovieMutation,
  getGenreDistinctFilterValuesQuery,
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
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.movies.items)),
    );
  }

  public getMovieById(id: string): Promise<Movie> {
    return firstValueFrom(
      this.apollo
        .query<{ movieById: Movie }>({
          ...getMovieByIdQuery(id),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.movieById)),
    );
  }

  public getMoviesPage(
    skip: number,
    take: number,
    sort: MovieSort,
    filters: MovieFilters,
    searchTerm?: string,
  ): Promise<MoviePage> {
    return firstValueFrom(
      this.apollo
        .query<{ movies: MoviePage }>({
          ...getMoviesPageQuery(skip, take, sort, filters, searchTerm),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.movies)),
    );
  }

  public getGenreDistinctFilterValues(): Promise<string[]> {
    return firstValueFrom(
      this.apollo
        .query<{ genreDistinctFilterValues: string[] }>({
          ...getGenreDistinctFilterValuesQuery,
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data?.genreDistinctFilterValues ?? [])),
    );
  }

  public async createMovie(request: CreateMovieRequestInput): Promise<void> {
    await firstValueFrom(
      this.apollo.mutate({
        ...createMovieMutation(request),
      }),
    );
  }

  public async updateMovie(request: UpdateMovieRequestInput): Promise<void> {
    await firstValueFrom(
      this.apollo.mutate({
        ...updateMovieMutation(request),
      }),
    );
  }

  public async deleteMovie(id: string): Promise<void> {
    await firstValueFrom(
      this.apollo.mutate(deleteMovieMutation(id)),
    );
  }
}
