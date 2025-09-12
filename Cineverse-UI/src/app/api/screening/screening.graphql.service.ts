import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { Screening } from '../../utils/types/api/screening';
import { GET_SCREENINGS_QUERY } from './screenings.operations';

@Injectable({providedIn: 'root'})
export class ScreeningGraphqlService {
  constructor(private apollo: Apollo) {}

  public getScreenings() : Observable<Screening[]>{
    return this.apollo.watchQuery({
      query: GET_SCREENINGS_QUERY,
      variables: {
        currentDate: new Date().toISOString().split('T')[0]
      },
      context: {
        allowAnonymous: true
      }
    }).valueChanges.pipe(
      map((result: any) => result.data.screenings.items),
    );
  }
}
