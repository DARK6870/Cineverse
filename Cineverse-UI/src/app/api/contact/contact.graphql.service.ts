import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Observable } from 'rxjs';
import { CREATE_CONTACT } from './contact.operations';

@Injectable({
  providedIn: 'root'
})

export class ContactGraphqlService {
  constructor(private apollo: Apollo) {}

  public createContactRequest(request: {
    firstName: string;
    lastName: string;
    email: string;
    subject: string;
    description: string;
  }): Observable<any> {
    return this.apollo.mutate({
      mutation: CREATE_CONTACT,
      variables: { request },
      context: {
        allowAnonymous: true
      }
    });
  }
}
