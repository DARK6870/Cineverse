import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import {
  AuthenticationResponse, ChangePasswordRequestInput,
  LoginRequestInput,
  RegisterRequestInput, RestorePasswordRequestInput
} from './authentication.graphql.types';
import {
  changePasswordMutation,
  confirmEmailMutation,
  deleteRefreshTokenMutation,
  generateAccessTokenMutation,
  loginUserMutation,
  registerUserMutation,
  resendEmailVerificationCodeMutation, restorePasswordMutation, sendRestorePasswordEmailMutation
} from './authentication.graphql';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationGraphqlService {
  constructor(private apollo: Apollo) {}

  public loginUser(request: LoginRequestInput): Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{login: AuthenticationResponse}>(
      loginUserMutation(request)
    ).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via login mutation');

        return result.data.login;
      }
    ));
  }

  public registerUser(request: RegisterRequestInput) : Observable<AuthenticationResponse>
  {
    return this.apollo.mutate<{register: AuthenticationResponse}>(
      registerUserMutation(request)
    ).pipe(
      map(result => {
        if (!result.data)
          throw new Error('No data retrieved via register mutation');

        return result.data.register;
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

  public deleteRefreshToken() : Observable<any> {
    return this.apollo.mutate(
      deleteRefreshTokenMutation
    );
  }

  public changePassword(request: ChangePasswordRequestInput) : Observable<any> {
    return this.apollo.mutate(
      changePasswordMutation(request)
    );
  }

  public sendRestorePasswordEmail(email: string) : Observable<any> {
    return this.apollo.mutate(
      sendRestorePasswordEmailMutation(email)
    );
  }

  public restorePassword(request: RestorePasswordRequestInput) : Observable<any> {
    return this.apollo.mutate(
      restorePasswordMutation(request)
    );
  }
}

// TODO: Add Invalid request, no active sessions found handler
