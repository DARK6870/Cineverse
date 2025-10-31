import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Observable } from 'rxjs';
import { CreateContactRequestInput } from './contact.graphql.types';
import {createContactRequestMutation} from './contact.graphql';

@Injectable({
  providedIn: 'root'
})

export class ContactGraphqlService {
  constructor(private apollo: Apollo) {}

  public createContactRequest(request: CreateContactRequestInput): Observable<any> {
    return this.apollo.mutate(
      createContactRequestMutation(request)
    );
  }
}
