import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { Movie } from './movie.graphql.types';
import {
  getComingSoonMovies,
  getMovieByIdQuery,
  getMoviesByIdsQuery,
} from './movie.graphql';

@Injectable({ providedIn: 'root' })
export class MovieGraphqlService {
  private apollo = inject(Apollo);

  public getMoviesByIds(ids: string[]): Observable<Movie[]> {
    return this.apollo
      .query<{ movies: { items: Movie[] } }>({
        ...getMoviesByIdsQuery(ids),
      })
      .pipe(map((res) => res.data.movies.items));
  }

  public getComingSoonMovies(): Observable<Movie[]> {
    return this.apollo
      .query<{ movies: { items: Movie[] } }>({
        ...getComingSoonMovies,
      })
      .pipe(map((res) => res.data.movies.items));
  }

  public getMovieById(id: string): Observable<Movie> {
    return this.apollo
      .query<{ movieById: Movie }>({
        ...getMovieByIdQuery(id),
      })
      .pipe(map((res) => res.data.movieById));
  }
}

// TODO: cache api requests
