import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  JwtPayload,
  UserStatus,
} from '../../../../shared/models/jwt-payload.model';
import { AuthenticationService } from '@cineverse/infrastructure-auth';

@Component({
  selector: 'app-profile',
  imports: [RouterLink],
  standalone: true,
  templateUrl: 'profile.html',
  styleUrl: 'profile.css',
})
export class Profile implements OnInit {
  private authenticationService = inject(AuthenticationService);

  userData = signal<JwtPayload | null>(null);

  async ngOnInit() {
    this.userData.set(await this.authenticationService.getUserDataAsync());
  }

  protected readonly UserStatus = UserStatus;
}
