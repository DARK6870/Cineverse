import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { getActiveScreeningMovieIdsQuery } from './screening.graphql';

@Injectable({providedIn: 'root'})
export class ScreeningGraphqlService {
  constructor(private apollo: Apollo) {}

  public getScreeningMovieIds() : Observable<string[]>{
    return this.apollo.watchQuery<{ screenings: { items: { movieId: string}[] } }>(
      getActiveScreeningMovieIdsQuery
    ).valueChanges.pipe(
      map((result => result.data.screenings.items.map(item => item.movieId)))
    );
  }
}
