import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../services/authentication/authentication.service';
import { JwtClaimsService } from '../../../services/jwt/jwt-claims.service';

@Component({
  selector: 'app-account',
  imports: [],
  templateUrl: 'account.html',
  styleUrl: 'account.css'
})

export class Account implements OnInit{

  constructor(
    private authenticationService: AuthenticationService,
    private jwtClaimService: JwtClaimsService
  ) {
  }

  async ngOnInit() {
    this.authenticationService.requireRefreshToken();
  }
}
