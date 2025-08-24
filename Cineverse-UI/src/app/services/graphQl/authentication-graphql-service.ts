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
} from '../../common/constants/graphql/authentication-operations';
import { AuthenticationResponse } from '../../common/models/authentication/authentication-response';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationGraphQlService {
  constructor(private apollo: Apollo) {}

  login(request: {
    email: string;
    password: string;
  }): Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{login: AuthenticationResponse}>({
      mutation: LOGIN_MUTATION,
      variables: { request }
    }).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via login mutation');

        return result.data.login;
      }
    ));
  }

  register(request: {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
  }) : Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{register: AuthenticationResponse}>({
      mutation: REGISTER_MUTATION,
      variables: { request }
    }).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via register mutation');

        return result.data.register;
      })
    );
  }

  confirmEmail(code: number, accessToken: string) : Observable<any> {
    return this.apollo.mutate({
      mutation: CONFIRM_EMAIL_MUTATION,
      variables: { code },
      context: {
        headers: {
          Authorization: `Bearer ${accessToken}`
        }
      }
    });
  }

  resendVerificationCode(accessToken: string) : Observable<any> {
    return this.apollo.mutate({
      mutation: RESEND_VERIFICATION_CODE_MUTATION,
      context: {
        headers: {
          Authorization: `Bearer ${accessToken}`
        }
      }
    });
  }

  generateAccessToken(refreshToken: string) : Observable<AuthenticationResponse> {
    return this.apollo.mutate<{generateAccessToken: AuthenticationResponse}>({
      mutation: GENERATE_ACCESS_TOKEN_MUTATION,
      variables: { refreshToken }
    }).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via generateAccessToken mutation');

        return result.data.generateAccessToken;
      })
    );
  }

  deleteRefreshToken(accessToken: string) : Observable<any> {
    return this.apollo.mutate({
      mutation: DELETE_REFRESH_TOKEN_MUTATION,
      context: {
        headers: {
          Authorization: `Bearer ${accessToken}`
        }
      }
    });
  }
}
