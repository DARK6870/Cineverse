import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { Movie } from './movie.graphql.types';
import { getMovieByIdQuery, getMoviesByIdsQuery } from './movie.graphql';

@Injectable({ providedIn: 'root'})
export class MovieGraphqlService {

  constructor(private apollo: Apollo) {}

  public getMoviesByIds(ids: string[]): Observable<Movie[]> {
    return this.apollo.watchQuery<{ movies : { items: Movie[] } }>(
      getMoviesByIdsQuery(ids)
    ).valueChanges.pipe(
      map(result => result.data.movies.items)
    );
  }

  public getMovieById(id: string): Observable<Movie> {
    return this.apollo.watchQuery<{ movieById: Movie }>(
      getMovieByIdQuery(id)
    ).valueChanges.pipe(
      map(result => result.data.movieById)
    );
  }
}
