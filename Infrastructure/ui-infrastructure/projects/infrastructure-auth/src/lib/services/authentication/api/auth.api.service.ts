import { inject, Injectable } from '@angular/core';
import { AuthenticationResponse, GenerateAccessTokenRequest } from './auth.api.types';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { HttpHeaderHelpers } from '../../../http/helpers/http-header.helper';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private http = inject(HttpClient);

  private baseApiPath = '/api/identity'

  public async generateAccessToken(request: GenerateAccessTokenRequest) : Promise<AuthenticationResponse> {
    return await firstValueFrom(
      this.http.post<AuthenticationResponse>(
        `${this.baseApiPath}/token/generate`,
        request,
        { headers: HttpHeaderHelpers.allowAnonymous() }
      )
    );
  }
}
