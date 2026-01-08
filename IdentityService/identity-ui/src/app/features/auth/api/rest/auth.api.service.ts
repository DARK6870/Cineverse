import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationResponse, LoginRequest, RegisterRequest } from './auth.api.types';
import { firstValueFrom } from 'rxjs';
import { HttpHeaderHelpers } from '@cineverse/infrastructure-auth';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private httpClient = inject(HttpClient);
  private baseApiPath = '/api/identity';

  public async loginAsync(request: LoginRequest){
    return await firstValueFrom(this.httpClient.post<AuthenticationResponse>(
      `${this.baseApiPath}/account/login`,
      request,
      { headers: HttpHeaderHelpers.allowAnonymous() }
      ));
  }

  public async registerAsync(request: RegisterRequest){
    return await firstValueFrom(this.httpClient.post<AuthenticationResponse>(
      `${this.baseApiPath}/account/register`,
      request,
      { headers: HttpHeaderHelpers.allowAnonymous() }
    ));
  }

  public async logoutAsync(){
    return await firstValueFrom(
      this.httpClient.post<AuthenticationResponse>(
        `${this.baseApiPath}/account/logout`,
        null
      )
    );
  }
}
