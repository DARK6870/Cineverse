import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import {
  REFRESH_TOKEN_KEY,
  ACCESS_TOKEN_KEY,
  REFRESH_TOKEN_LIFETIME_DAYS,
  ACCESS_TOKEN_LIFETIME_MINUTES
} from '../../common/constants/cookie/cookie-constants';
import { AuthenticationGraphQlService } from '../graphQl/authentication-graphql-service';
import { AuthenticationResponse } from '../../common/models/authentication/authentication-response';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationService {

  constructor(
    private cookieService: CookieService,
    private authenticationGraphQlService: AuthenticationGraphQlService,
    private router: Router,
    private route: ActivatedRoute,
    private messageService: MessageService
  ) {
  }

  loginUser(request: {
    email: string;
    password: string;
  })
  {
    this.authenticationGraphQlService
      .login(request)
      .subscribe({
        next: (loginResponse: AuthenticationResponse) => {
          // Save tokens to cookie
          this.cookieService.set(REFRESH_TOKEN_KEY, loginResponse.refreshToken, REFRESH_TOKEN_LIFETIME_DAYS);
          this.cookieService.set(ACCESS_TOKEN_KEY, loginResponse.accessToken, ACCESS_TOKEN_LIFETIME_MINUTES);

          // Redirect
          const callbackUrl = this.route.snapshot.queryParamMap.get('callbackUrl') || '/';

          this.router.navigate([callbackUrl]).then(() => {
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Successfully logged in' });
          })
        }})
  }

  registerUser(request: {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
  })
  {
    this.authenticationGraphQlService
      .register(request)
      .subscribe({
        next: (loginResponse: AuthenticationResponse) => {
          // Save refresh token to cookie
          this.saveAccessTokenToCookie(loginResponse.accessToken);
          this.saveRefreshTokenToCookie(loginResponse.refreshToken);

          // Redirect
          this.router.navigate(['/confirm-email/true']).then(() => {
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Account created successfully' });
          });
        }
      })
  }

  // Delete refresh token and remove refresh and access tokens from cookie
  async logoutUserAsync() {
    const accessToken = await this.getAccessTokenAsync();

    this.authenticationGraphQlService
      .deleteRefreshToken(accessToken)
      .subscribe({
        next: () => {
          this.cookieService.delete(REFRESH_TOKEN_KEY);
          this.cookieService.delete(ACCESS_TOKEN_KEY);

          this.router.navigate(['/login']).then(() => {
            this.messageService.add({ severity: 'info', summary: 'Logout', detail: 'You have been logged out' });
          });
        }
      })
  }

  // Get refresh token from cookie
  // If token == null -> redirect to login page
  requireRefreshToken(): string {
    const refreshToken = this.cookieService.get(REFRESH_TOKEN_KEY);

    if (!refreshToken) {
      const callbackUrl = this.router.url;
      this.router.navigate(
        ['/login'],
        { queryParams: {callbackUrl} }
      ).then(() => {
        this.messageService.add({severity: 'info', summary: 'Authorization required', detail: 'Please login into your account'});
      });
      throw new Error('Authorization required');
    }

    return refreshToken;
  }

  // Get access token from cookie or generate a new one using refresh token
  async getAccessTokenAsync(): Promise<string> {
    const refreshToken = this.requireRefreshToken();

    const cookieAccessToken = this.cookieService.get(ACCESS_TOKEN_KEY);
    if (cookieAccessToken)
      return cookieAccessToken;

    const loginResponse = await firstValueFrom(
      this.authenticationGraphQlService.generateAccessToken(refreshToken)
    );

    if (!loginResponse.success)
      throw new Error(loginResponse.message);

    this.saveAccessTokenToCookie(loginResponse.accessToken);
    return loginResponse.accessToken;
  }

  ensureUserNotAuthorized(): boolean{
    const refreshToken = this.cookieService.get(REFRESH_TOKEN_KEY);

    if (refreshToken) {
      this.router.navigate(['/account']).then(() => {
        this.messageService.add({severity: 'info', summary: 'Already authorized', detail: 'You are already authorized'});
      });

      return false;
    }

    return true;
  }

  // Save access token to cookie
  private saveAccessTokenToCookie(accessToken: string) {
    const expires = new Date();
    expires.setMinutes(expires.getMinutes() + ACCESS_TOKEN_LIFETIME_MINUTES);

    this.cookieService.set(
      ACCESS_TOKEN_KEY,
      accessToken,
      expires,
      '/',
      '', // Current domain
      false, // SSL
      'Strict'
    );
  }

  // Save refresh token to cookie
  private saveRefreshTokenToCookie(refreshToken: string) {
    this.cookieService.set(
      REFRESH_TOKEN_KEY,
      refreshToken,
      REFRESH_TOKEN_LIFETIME_DAYS,
      '/',
      '', // Current domain
      false, // SSL
      'Strict'
    );
  }
}
