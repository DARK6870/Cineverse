import { CanActivateFn } from '@angular/router';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { inject } from '@angular/core';
import { RouterHelper, ToastService } from '@cineverse/infrastructure-common';
import { UserStatus } from '../shared/models/user-data';

export const userStatusGuard = (
  requiredStatus: UserStatus,
  toastMessage: string
): CanActivateFn => {
  return async (route, state) => {
    const routerHelper = inject(RouterHelper);
    const authenticationService = inject(AuthenticationService);
    const toastService = inject(ToastService);

    const userData = await authenticationService.getUserDataAsync();

    if (userData.userStatus !== requiredStatus) {
      toastService.warning(toastMessage);
      return false;
    }

    return true;
  };
};
