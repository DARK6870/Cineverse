import { CanActivate, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { ToastService } from '../services/toast.service';
import { inject, Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class NotAuthorizedGuard implements CanActivate {
  private router = inject(Router);
  private authenticationService = inject(AuthenticationService);
  private toastService = inject(ToastService);

  canActivate(): boolean {
    if (!this.authenticationService.isAuthenticated()) {
      return true;
    } else {
      this.router.navigate(['/profile']).then(() => {
        this.toastService.info('You are already authorized');
      });

      return false;
    }
  }
}
