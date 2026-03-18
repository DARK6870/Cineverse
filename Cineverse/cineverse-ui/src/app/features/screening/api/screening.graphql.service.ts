import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map, pipe } from 'rxjs';
import {
  getActiveScreeningMovieIdsQuery,
  getActiveScreeningsByMovieIdQuery,
  getScreeningByIdQuery, getScreeningsByIds
} from './screening.graphql';
import { Screening } from './screening.graphql.types';

@Injectable({ providedIn: 'root' })
export class ScreeningGraphqlService {
  private apollo = inject(Apollo);

  public getScreeningMovieIds(): Promise<string[]> {
    return firstValueFrom(
      this.apollo
        .query<{ screenings: { items: { movieId: string }[] } }>({
          ...getActiveScreeningMovieIdsQuery,
        })
        .pipe(map((res) => res.data.screenings.items.map((i) => i.movieId)))
    );
  }

  public getActiveScreeningsForMovie(id: string): Promise<Screening[]> {
    return firstValueFrom(
      this.apollo
        .query<{ screenings: { items: Screening[] } }>({
          ...getActiveScreeningsByMovieIdQuery(id),
        })
        .pipe(map((res) => res.data.screenings.items))
    );
  }

  public getScreeningsByIds(ids: string[]): Promise<Screening[]> {
    return firstValueFrom(
      this.apollo
        .query<{ screenings: { items: Screening[] } }>({
          ...getScreeningsByIds(ids),
        })
        .pipe(map((res) => res.data.screenings.items)),
    );
  }

  public getScreeningById(id: string): Promise<Screening> {
    return firstValueFrom(
      this.apollo
        .query<{ screeningById: Screening }>({
          ...getScreeningByIdQuery(id),
        })
        .pipe(map((result) => result.data.screeningById)),
    );
  }
}
