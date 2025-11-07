import { Component, OnInit, signal } from '@angular/core';
import { AuthenticationService } from '../../../../core/services/authentication.service';
import { RouterLink } from '@angular/router';
import { JwtClaimsService } from '../../../../core/services/jwt-claims.service';
import {JwtPayload, UserStatus} from '../../../../shared/models/jwt-payload.model';

@Component({
  selector: 'app-profile',
  imports: [
    RouterLink
  ],
  standalone: true,
  templateUrl: 'profile.html',
  styleUrl: 'profile.css'
})

export class Profile implements OnInit{
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
