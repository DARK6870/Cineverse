import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Observable } from 'rxjs';
import { CREATE_SUPPORT_TICKET_MUTATION } from '../constants/graphql/support.operations';

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
      mutation: CREATE_SUPPORT_TICKET_MUTATION,
      variables: { request }
    });
  }
}
