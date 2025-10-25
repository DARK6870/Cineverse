import { Component, OnInit, signal } from '@angular/core';
import { AuthenticationService } from '../../services/authentication/authentication.service';
import { RouterLink } from '@angular/router';
import { JwtClaimsService } from '../../services/jwt-claims/jwt-claims.service';
import {JwtPayload, UserStatus} from '../../utils/models/jwt-payload.model';

@Component({
  selector: 'app-account',
  imports: [
    RouterLink
  ],
  templateUrl: 'account.html',
  styleUrl: 'account.css'
})

export class Account implements OnInit{
  userData =  signal<JwtPayload | null>(null);

  constructor(
    private authenticationService: AuthenticationService,
    private jwtClaimsService: JwtClaimsService
  ) {
  }

  async ngOnInit() {
    this.userData.set(await this.jwtClaimsService.decodeTokenAsync());
  }

  protected readonly UserStatus = UserStatus;
}
