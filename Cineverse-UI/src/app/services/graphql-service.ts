import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable, of } from 'rxjs';
import { CREATE_CONTACT } from '../common/constants/graphql/contact-operations';
import { GET_MOVIES } from '../common/constants/graphql/movie-operations';
import { Movie } from '../common/models/movie';
import { Screening } from '../common/models/screening';
import { GET_SCREENINGS } from '../common/constants/graphql/screenings-operations';

@Injectable({
  providedIn: 'root'
})
export class GraphqlService {
  constructor(private apollo: Apollo) {}

  createSupportTicket(request: {
    firstName: string;
    lastName: string;
    email: string;
    subject: string;
    description: string;
  }): Observable<any> {
    return this.apollo.mutate({
      mutation: CREATE_CONTACT,
      variables: { request }
    });
  }

  getMovies(ids: string[]): Observable<Movie[]> {
    return this.apollo.watchQuery({
      query: GET_MOVIES,
      variables: {
        ids: ids
      }
    }).valueChanges.pipe(
      map((result: any) => result.data.movies.items),
    );
  }

  getScreenings() : Observable<Screening[]>{
    return this.apollo.watchQuery({
      query: GET_SCREENINGS,
      variables: {
        currentDate: new Date().toISOString().split('T')[0]
      }
    }).valueChanges.pipe(
      map((result: any) => result.data.screenings.items),
    );
  }
}
