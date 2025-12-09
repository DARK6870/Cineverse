import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import {
  getActiveScreeningMovieIdsQuery,
  getActiveScreeningsByMovieIdQuery,
  getScreeningByIdQuery
} from './screening.graphql';
import { Screening } from './screening.graphql.types';

@Injectable({providedIn: 'root'})
export class ScreeningGraphqlService {
  private apollo = inject(Apollo);

  // TODO: Change to query...
  public getScreeningMovieIds() : Observable<string[]>{
    return this.apollo.watchQuery<{ screenings: { items: { movieId: string}[] } }>(
      getActiveScreeningMovieIdsQuery
    ).valueChanges.pipe(
      map((result => result.data.screenings.items.map(item => item.movieId)))
    );
  }

  public getActiveScreeningsForMovie(id: string): Observable<Screening[]>{
    return this.apollo.watchQuery<{ screenings: { items: Screening[] } }>(
      getActiveScreeningsByMovieIdQuery(id)
    ).valueChanges.pipe(
      map((result => result.data.screenings.items))
    );
  };

  public getScreeningById(id: string): Observable<Screening> {
    return this.apollo.watchQuery<{ screeningById: Screening }>(
      getScreeningByIdQuery(id)
    ).valueChanges.pipe(
      map((result => result.data.screeningById))
    )
  }
}
