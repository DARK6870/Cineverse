import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import {map, Observable, of} from 'rxjs';
import { CREATE_CONTACT } from '../common/constants/graphql/contact-operations';
import { GET_MOVIES } from '../common/constants/graphql/movie-operations';
import {catchError} from 'rxjs/operators';
import {Movie} from '../common/models/movie';

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

  getMovies(): Observable<Movie[]> {
    return this.apollo.watchQuery({
      query: GET_MOVIES
    }).valueChanges.pipe(
      map((result: any) => result.data.movies.items),
      catchError(error => {
        console.error('Error fetching movies:', error);
        return of([]);
      })
    );
  }
}
