import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { Movie } from './movie.graphql.types';
import { getMovieByIdQuery, getMoviesByIdsQuery } from './movie.graphql';

@Injectable({
  providedIn: 'root'
})

export class MovieGraphqlService {
  constructor(private apollo: Apollo) {}

  public getMoviesByIds(ids: string[]): Observable<Movie[]> {
    return this.apollo.watchQuery(
      getMoviesByIdsQuery(ids)
    ).valueChanges.pipe(
      map((result: any) => result.data.movies.items)
    );
  }

  public getMovieById(id: string): Observable<Movie> {
    return this.apollo.watchQuery(
      getMovieByIdQuery(id)
    ).valueChanges.pipe(
      map((result: any) => result.data.movieById)
    );
  }
}
