import { Component, inject, OnInit } from '@angular/core';
import { TokenStorageService } from '@cineverse/infrastructure-auth';
import { AuthApiService } from '../../api/rest/auth.api.service';
import { Router } from '@angular/router';
import { ToastService } from '@cineverse/infrastructure-common';

@Component({
  selector: 'app-logout',
  imports: [],
  standalone: true,
  templateUrl: 'logout.html',
  styleUrl: 'logout.css',
})
export class Logout implements OnInit {
  private tokenStorageService = inject(TokenStorageService);
  private authApiService = inject(AuthApiService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  async ngOnInit() {
    await this.authApiService.logoutAsync();
    this.tokenStorageService.deleteTokens();

    this.router.navigate(['/login']).then(() => {
      this.toastService.info('You have been logged out');
    });
  }
}
