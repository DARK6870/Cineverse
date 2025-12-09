import { inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import {
  ACCESS_TOKEN_KEY,
  ACCESS_TOKEN_LIFETIME_MINUTES,
  REFRESH_TOKEN_KEY,
  REFRESH_TOKEN_LIFETIME_DAYS,
} from '../../shared/constants/cookie-constants';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  private cookieService = inject(CookieService);

  public getAccessToken(): string {
    return this.cookieService.get(ACCESS_TOKEN_KEY);
  }

  public getRefreshToken(): string {
    return this.cookieService.get(REFRESH_TOKEN_KEY);
  }

  public saveAccessToken(accessToken: string) {
    const expires = new Date();
    expires.setMinutes(expires.getMinutes() + ACCESS_TOKEN_LIFETIME_MINUTES);

    this.cookieService.set(
      ACCESS_TOKEN_KEY,
      accessToken,
      expires,
      '/cineverse',
      '',
      false,
      'Strict',
    );
  }

  public saveRefreshToken(refreshToken: string) {
    this.cookieService.set(
      REFRESH_TOKEN_KEY,
      refreshToken,
      REFRESH_TOKEN_LIFETIME_DAYS,
      '/cineverse',
      '',
      false,
      'Strict',
    );
  }

  public deleteAccessToken() {
    this.cookieService.delete(ACCESS_TOKEN_KEY, '/cineverse');
  }

  public deleteRefreshToken() {
    this.cookieService.delete(REFRESH_TOKEN_KEY, '/cineverse');
  }

  public deleteTokens() {
    this.deleteAccessToken();
    this.deleteRefreshToken();
  }
}
