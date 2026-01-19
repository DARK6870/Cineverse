import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { identityBasePath, RouterHelper, ToastService } from '@cineverse/infrastructure-common';

export const authenticationGuard: CanActivateFn = (state) => {
  const routerHelper = inject(RouterHelper);
  const authenticationService = inject(AuthenticationService);
  const toastService = inject(ToastService);

  if (authenticationService.isAuthenticated()) {
    return true;
  }

  toastService.info('Please login into your account');

  routerHelper.navigate(`${identityBasePath}/login`, {
    queryParams: {
      callbackUrl: state.url
    }
  });

  return false;
};
