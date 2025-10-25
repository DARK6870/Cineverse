import { CanActivate, Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { ToastService } from '../../services/toast/toast.service';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class NotAuthorizedGuard implements CanActivate {

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private toastService: ToastService
  ) {

  }

  canActivate(): boolean {
    if (!this.authenticationService.isAuthenticated()) {
      return true;
    } else {
      this.router.navigate(['/account']).then(() => {
        this.toastService.info('You are already authorized');
      });

      return false;
    }
  }
}
