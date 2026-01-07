import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { AuthenticationResponse } from './auth.graphql.types';
import { generateAccessTokenMutation } from './auth.graphql';

@Injectable({ providedIn: 'root' })
export class AuthGraphqlService {
  private apollo = inject(Apollo);

  public generateAccessToken(refreshToken: string) : Observable<AuthenticationResponse> {
    return this.apollo.mutate<{generateAccessToken: AuthenticationResponse}>(
      generateAccessTokenMutation(refreshToken)
    ).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via generateAccessToken mutation');

        return result.data.generateAccessToken;
      })
    );
  }
}
