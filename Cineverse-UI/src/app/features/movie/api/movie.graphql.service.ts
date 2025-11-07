import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { Movie } from './movie.graphql.types';
import {getComingSoonMovies, getMovieByIdQuery, getMoviesByIdsQuery} from './movie.graphql';

@Injectable({ providedIn: 'root'})
export class MovieGraphqlService {

  constructor(private apollo: Apollo) {}

  public getMoviesByIds(ids: string[]): Observable<Movie[]> {
    return this.apollo.query<{ movies : { items: Movie[] } }>({
      ...getMoviesByIdsQuery(ids),
      fetchPolicy: 'network-only'
    }).pipe(map(res => res.data.movies.items));
  }

  public getComingSoonMovies(): Observable<Movie[]> {
    return this.apollo.query<{ movies : { items: Movie[] } }>({
      ...getComingSoonMovies,
      fetchPolicy: 'network-only'
    }).pipe(map(res => res.data.movies.items));
  }

  public getMovieById(id: string): Observable<Movie> {
    return this.apollo.query<{ movieById: Movie }>({
      ...getMovieByIdQuery(id),
      fetchPolicy: 'cache-first'
    }).pipe(map(res => res.data.movieById));
  }
}
