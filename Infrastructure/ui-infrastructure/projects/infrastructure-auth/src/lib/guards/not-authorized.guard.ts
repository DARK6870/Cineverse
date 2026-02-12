import { CanActivateFn } from '@angular/router';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { inject } from '@angular/core';
import { RouterHelper, ToastService, identityBasePath } from '@cineverse/infrastructure-common';

export const notAuthorizedGuard : CanActivateFn = () => {
  const routerHelper = inject(RouterHelper);
  const authenticationService = inject(AuthenticationService);
  const toastService = inject(ToastService);

  if (!authenticationService.isAuthenticated()) {
    return true;
  } else {
    toastService.info('You are already authorized');
    routerHelper.navigate(`${identityBasePath}/profile`);

    return false;
  }
};
