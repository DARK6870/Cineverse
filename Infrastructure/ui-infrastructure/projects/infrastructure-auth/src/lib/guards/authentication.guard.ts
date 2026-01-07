import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { ToastService } from '@cineverse/infrastructure-common';

export const authenticationGuard: CanActivateFn = (state) => {
  const router = inject(Router);
  const authenticationService = inject(AuthenticationService);
  const toastService = inject(ToastService);

  if (authenticationService.isAuthenticated()) {
    return true;
  }

  toastService.info('Please login into your account');

  return router.createUrlTree(
    ['/login'],
    { queryParams: { callbackUrl: state.url } }
  );
};
