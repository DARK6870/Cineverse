import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Observable } from 'rxjs';
import {
  ChangePasswordRequestInput,
  RestorePasswordRequestInput
} from './auth.graphql.types';
import {
  changePasswordMutation,
  confirmEmailMutation,
  resendEmailVerificationCodeMutation,
  restorePasswordMutation,
  sendRestorePasswordEmailMutation
} from './auth.graphql';

@Injectable({ providedIn: 'root' })
export class AuthGraphqlService {
  private apollo = inject(Apollo);

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
