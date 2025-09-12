import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import {
  AuthenticationResponse,
  LoginRequestInput,
  RegisterRequestInput
} from './authentication.graphql.types';
import {
  confirmEmailMutation,
  deleteRefreshTokenMutation,
  generateAccessTokenMutation,
  loginUserMutation,
  registerUserMutation,
  resendEmailVerificationCodeMutation
} from './authentication.graphql';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationGraphqlService {
  constructor(private apollo: Apollo) {}

  public loginUser(request: LoginRequestInput): Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{authenticationResponse: AuthenticationResponse}>(
      loginUserMutation(request)
    ).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via login mutation');

        return result.data.authenticationResponse;
      }
    ));
  }

  public registerUser(request: RegisterRequestInput) : Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{authenticationResponse: AuthenticationResponse}>(
      registerUserMutation(request)
    ).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via register mutation');

        return result.data.authenticationResponse;
      })
    );
  }

  public confirmEmail(code: number) : Observable<any> {
    return this.apollo.mutate(
      confirmEmailMutation(code)
    );
  }

  public resendEmailVerificationCode() : Observable<any> {
    return this.apollo.mutate(
      resendEmailVerificationCodeMutation
    );
  }

  public generateAccessToken(refreshToken: string) : Observable<AuthenticationResponse> {
    return this.apollo.mutate<{authenticationResponse: AuthenticationResponse}>(
      generateAccessTokenMutation(refreshToken)
    ).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via generateAccessToken mutation');

        return result.data.authenticationResponse;
      })
    );
  }

  public deleteRefreshToken() : Observable<any> {
    return this.apollo.mutate(
      deleteRefreshTokenMutation
    );
  }
}
