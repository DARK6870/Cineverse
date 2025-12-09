import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Observable } from 'rxjs';
import { CreateContactRequestInput } from './contact.graphql.types';
import { createContactRequestMutation } from './contact.graphql';

@Injectable({
  providedIn: 'root'
})

export class ContactGraphqlService {
  private apollo = inject(Apollo);

  public createContactRequest(request: CreateContactRequestInput): Observable<any> {
    return this.apollo.mutate(
      createContactRequestMutation(request)
    );
  }
}
