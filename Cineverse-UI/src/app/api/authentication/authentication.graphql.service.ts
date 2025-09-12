import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import {
  LOGIN_MUTATION,
  REGISTER_MUTATION,
  CONFIRM_EMAIL_MUTATION,
  RESEND_VERIFICATION_CODE_MUTATION,
  GENERATE_ACCESS_TOKEN_MUTATION,
  DELETE_REFRESH_TOKEN_MUTATION
} from './authentication.operations';
import { AuthenticationResponse } from '../../utils/types/api/authentication';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationGraphqlService {
  constructor(private apollo: Apollo) {}

  public loginUser(request: {
    email: string;
    password: string;
  }): Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{login: AuthenticationResponse}>({
      mutation: LOGIN_MUTATION,
      variables: { request },
      context: {
        allowAnonymous: true
      }
    }).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via login mutation');

        return result.data.login;
      }
    ));
  }

  public registerUser(request: {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
  }) : Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{register: AuthenticationResponse}>({
      mutation: REGISTER_MUTATION,
      variables: { request },
      context: {
        allowAnonymous: true
      }
    }).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via register mutation');

        return result.data.register;
      })
    );
  }

  public confirmEmail(code: number) : Observable<any> {
    return this.apollo.mutate({
      mutation: CONFIRM_EMAIL_MUTATION,
      variables: { code }
    });
  }

  public resendEmailVerificationCode() : Observable<any> {
    return this.apollo.mutate({
      mutation: RESEND_VERIFICATION_CODE_MUTATION
    });
  }

  public generateAccessToken(refreshToken: string) : Observable<AuthenticationResponse> {
    return this.apollo.mutate<{generateAccessToken: AuthenticationResponse}>({
      mutation: GENERATE_ACCESS_TOKEN_MUTATION,
      variables: { refreshToken },
      context: {
        allowAnonymous: true
      }
    }).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via generateAccessToken mutation');

        return result.data.generateAccessToken;
      })
    );
  }

  public deleteRefreshToken() : Observable<any> {
    return this.apollo.mutate({
      mutation: DELETE_REFRESH_TOKEN_MUTATION
    });
  }
}
