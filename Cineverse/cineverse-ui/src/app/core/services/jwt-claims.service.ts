import { inject, Injectable } from '@angular/core';
import { AuthenticationService } from './authentication.service';
import { JwtPayload } from '../../shared/models/jwt-payload.model';

@Injectable({ providedIn: 'root' })
export class JwtClaimsService {

  private authenticationService = inject(AuthenticationService);

  public async decodeTokenAsync(): Promise<JwtPayload> {
    const accessToken = await this.authenticationService.getOrGenerateAccessTokenAsync();
    const decoded = this.decodeTokenPayload(accessToken);
    return {
      userId: decoded.user_id,
      fullName: decoded.unique_name,
      email: decoded.email,
      role: decoded.role,
      userStatus: decoded.user_status,
    };
  }

  private decodeTokenPayload(token: string): any {
    const payloadBase64 = token.split('.')[1];
    const payloadJson = decodeURIComponent(
      atob(payloadBase64)
        .split('')
        .map(c => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
        .join('')
    );
    return JSON.parse(payloadJson);
  }
}
