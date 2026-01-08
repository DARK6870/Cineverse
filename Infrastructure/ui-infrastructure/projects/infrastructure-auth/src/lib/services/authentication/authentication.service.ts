import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthApiService } from './api/auth.api.service';
import { TokenStorageService } from '../tokenStorage/token-storage.service';
import { ToastService } from '@cineverse/infrastructure-common';
import { UserData } from '../../shared/models/user-data';
import { decodeTokenPayload } from '../../shared/helpers/jwt.helper';
import {GenerateAccessTokenRequest} from './api/auth.api.types';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private authApiService = inject(AuthApiService);
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
    const request : GenerateAccessTokenRequest = {
      refreshToken: refreshToken,
    };

    try {
      const loginResponse = await this.authApiService.generateAccessToken(request);

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

  public async getUserDataAsync(): Promise<UserData> {
    const accessToken = await this.getOrGenerateAccessTokenAsync();
    const decoded = decodeTokenPayload(accessToken);
    return {
      userId: decoded.user_id,
      fullName: decoded.unique_name,
      email: decoded.email,
      role: decoded.role,
      userStatus: decoded.user_status,
    };
  }
}
