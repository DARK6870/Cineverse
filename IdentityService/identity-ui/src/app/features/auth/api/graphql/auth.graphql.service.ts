import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom } from 'rxjs';
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

  public async confirmEmailAsync(code: number) {
    return firstValueFrom(
      this.apollo.mutate(confirmEmailMutation(code))
    );
  }

  public async resendEmailVerificationCodeAsync() {
    return await firstValueFrom(
      this.apollo.mutate(resendEmailVerificationCodeMutation)
    );
  }

  public async changePasswordAsync(request: ChangePasswordRequestInput) {
    return await firstValueFrom(
      this.apollo.mutate(changePasswordMutation(request))
    );
  }

  public async sendRestorePasswordEmailAsync(email: string) {
    return await firstValueFrom(
      this.apollo.mutate(sendRestorePasswordEmailMutation(email))
    );
  }

  public async restorePasswordAsync(request: RestorePasswordRequestInput) {
    return await firstValueFrom(
      this.apollo.mutate(restorePasswordMutation(request))
    );
  }
}
