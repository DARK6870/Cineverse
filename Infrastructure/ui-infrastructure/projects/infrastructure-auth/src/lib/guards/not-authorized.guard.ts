import { CanActivateFn, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication/authentication.service';
import { inject } from '@angular/core';
import { ToastService } from '@cineverse/infrastructure-common';

export const notAuthorizedGuard : CanActivateFn = () => {
  const router = inject(Router);
  const authenticationService = inject(AuthenticationService);
  const toastService = inject(ToastService);

  if (!authenticationService.isAuthenticated()) {
    return true;
  } else {
    toastService.info('You are already authorized');
    return router.createUrlTree(['/profile']);
  }
};
