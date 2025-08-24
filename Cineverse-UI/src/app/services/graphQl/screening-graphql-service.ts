import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { Screening } from '../../common/models/screening/screening';
import { GET_SCREENINGS_QUERY } from '../../common/constants/graphql/screenings-operations';

@Injectable({
  providedIn: 'root'
})

export class ScreeningGraphQlService {
  constructor(private apollo: Apollo) {}

  getScreenings() : Observable<Screening[]>{
    return this.apollo.watchQuery({
      query: GET_SCREENINGS_QUERY,
      variables: {
        currentDate: new Date().toISOString().split('T')[0]
      }
    }).valueChanges.pipe(
      map((result: any) => result.data.screenings.items),
    );
  }
}
