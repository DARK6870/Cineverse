import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { AuthGraphqlService } from './api/auth.graphql.service';
import { TokenStorageService } from '../tokenStorage/token-storage.service';
import { ToastService } from '@cineverse/infrastructure-common';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private authenticationGraphQlService = inject(AuthGraphqlService);
  private tokenStorageService = inject(TokenStorageService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  public isAuthenticated(): boolean {
    return !!this.tokenStorageService.getRefreshToken();
  }

  public requireRefreshToken(): string {
    const refreshToken = this.tokenStorageService.getRefreshToken();

    if (!refreshToken) {
      const callbackUrl = this.router.url;
      this.router.navigate(
        ['/login'],
        { queryParams: {callbackUrl} }
      ).then(() => {
        this.toastService.info('Please login into your profile');
      });
      throw new Error('Authorization required');
    }

    return refreshToken;
  }

  public async getOrGenerateAccessTokenAsync(): Promise<string> {
    this.requireRefreshToken();

    const cookieAccessToken = this.tokenStorageService.getAccessToken();
    if (cookieAccessToken)
      return cookieAccessToken;

    return await this.generateAccessTokenAsync();
  }

  public async generateAccessTokenAsync(): Promise<string> {
    const refreshToken = this.requireRefreshToken();

    try {
      const loginResponse = await firstValueFrom(
        this.authenticationGraphQlService.generateAccessToken(refreshToken)
      );

      if (!loginResponse.success)
        throw new Error(loginResponse.message);

      this.tokenStorageService.saveAccessToken(loginResponse.accessToken);
      return loginResponse.accessToken;
    }
    catch (error: any)
    {
      if (error.networkError?.statusCode === 401) {
        this.tokenStorageService.deleteTokens();
        this.requireRefreshToken();
      }

      throw error;
    }
  }
}
