import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JwtClaimsService } from '../../../../core/services/jwt-claims.service';
import {
  JwtPayload,
  UserStatus,
} from '../../../../shared/models/jwt-payload.model';

@Component({
  selector: 'app-profile',
  imports: [RouterLink],
  standalone: true,
  templateUrl: 'profile.html',
  styleUrl: 'profile.css',
})
export class Profile implements OnInit {
  private jwtClaimsService = inject(JwtClaimsService);

  userData = signal<JwtPayload | null>(null);

  async ngOnInit() {
    this.userData.set(await this.jwtClaimsService.decodeTokenAsync());
  }

  protected readonly UserStatus = UserStatus;
}
