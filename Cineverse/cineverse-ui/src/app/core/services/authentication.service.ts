import { inject, Injectable } from '@angular/core';
import { AuthGraphqlService } from '../../features/auth/api/auth.graphql.service';
import { Router, ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import {
  AuthenticationResponse, ChangePasswordRequestInput,
  RestorePasswordRequestInput
} from '../../features/auth/api/auth.graphql.types';
import { ToastService } from 'infrastructure-common';
import { TokenStorageService } from '@cineverse/infrastructure-auth';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private authenticationGraphQlService = inject(AuthGraphqlService);
  private tokenStorageService = inject(TokenStorageService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private toastService = inject(ToastService);


  public loginUser(request: {
    email: string;
    password: string;
  })
  {
    this.authenticationGraphQlService
      .loginUser(request)
      .subscribe({
        next: (loginResponse: AuthenticationResponse) => {
          this.tokenStorageService.saveRefreshToken(loginResponse.refreshToken);
          this.tokenStorageService.saveAccessToken(loginResponse.accessToken);

          const callbackUrl = this.route.snapshot.queryParamMap.get('callbackUrl') || '/';

          this.router.navigate([callbackUrl]).then(() => {
            this.toastService.success('Successfully logged in');
          })
        }})
  }

  public registerUser(request: {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
  })
  {
    this.authenticationGraphQlService
      .registerUser(request)
      .subscribe({
        next: (loginResponse: AuthenticationResponse) => {
          this.tokenStorageService.saveAccessToken(loginResponse.accessToken);
          this.tokenStorageService.saveRefreshToken(loginResponse.refreshToken);

          this.router.navigate(['/confirm-email/true']).then(() => {
            this.toastService.success('Profile created successfully');
          });
        }
      })
  }

  public async logoutUserAsync() {
    this.authenticationGraphQlService
      .deleteRefreshToken()
      .subscribe({
        next: () => {
          this.tokenStorageService.deleteTokens();

          this.router.navigate(['/login']).then(() => {
            this.toastService.info('You have been logged out');
          });
        }
      })
  }

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

  public async restorePasswordAsync(request: RestorePasswordRequestInput) : Promise<void> {
    await firstValueFrom(
      this.authenticationGraphQlService.restorePassword(request)
    );

    this.tokenStorageService.deleteTokens();
  }

  public async changePasswordAsync(request: ChangePasswordRequestInput) : Promise<void> {
    await firstValueFrom(
      this.authenticationGraphQlService.changePassword(request)
    );

    this.tokenStorageService.deleteTokens();
  }
}
