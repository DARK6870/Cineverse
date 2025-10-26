import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { UpdatePersonalInformationRequestInput } from './user.graphql.types';
import { Observable } from 'rxjs';
import { updatePersonalInformationMutation } from './user.graphql';

@Injectable({ providedIn: 'root' })
export class UserGraphqlService {
  constructor(private apollo: Apollo) {
  }

  public updatePersonalInformation(request: UpdatePersonalInformationRequestInput) : Observable<any> {
    return this.apollo.mutate(
      updatePersonalInformationMutation(request)
    );
  }
}
