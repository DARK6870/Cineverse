import { Injectable } from '@angular/core';
import { AuthenticationGraphqlService } from '../../api/authentication/authentication.graphql.service';
import { Router, ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { TokenStorageService } from './token-storage.service';
import { AuthenticationResponse } from '../../api/authentication/authentication.graphql.types';
import { ToastService } from '../toast/toast.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {

  constructor(
    private authenticationGraphQlService: AuthenticationGraphqlService,
    private tokenStorageService: TokenStorageService,
    private router: Router,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) {
  }

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
            this.toastService.success('Account created successfully');
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

  public requireRefreshToken(): string {
    const refreshToken = this.tokenStorageService.getRefreshToken();

    if (!refreshToken) {
      const callbackUrl = this.router.url;
      this.router.navigate(
        ['/login'],
        { queryParams: {callbackUrl} }
      ).then(() => {
        this.toastService.info('Please login into your account');
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

    return await this.regenerateAccessTokenAsync();
  }

  public async regenerateAccessTokenAsync(): Promise<string> {
    const refreshToken = this.requireRefreshToken();

    const loginResponse = await firstValueFrom(
      this.authenticationGraphQlService.generateAccessToken(refreshToken)
    );

    if (!loginResponse.success)
      throw new Error(loginResponse.message);

    this.tokenStorageService.saveAccessToken(loginResponse.accessToken);
    return loginResponse.accessToken;
  }

  public ensureUserNotAuthorized(): boolean{
    const refreshToken = this.tokenStorageService.getRefreshToken();

    if (refreshToken) {
      this.router.navigate(['/account']).then(() => {
        this.toastService.info('You are already authorized');
      });

      return false;
    }

    return true;
  }
}
