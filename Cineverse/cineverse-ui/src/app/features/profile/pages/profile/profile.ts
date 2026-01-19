import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthenticationService, UserData, UserStatus } from '@cineverse/infrastructure-auth';

@Component({
  selector: 'app-profile',
  imports: [RouterLink],
  standalone: true,
  templateUrl: 'profile.html',
  styleUrl: 'profile.css',
})
export class Profile implements OnInit {
  private authenticationService = inject(AuthenticationService);

  userData = signal<UserData | null>(null);

  async ngOnInit() {
    this.userData.set(await this.authenticationService.getUserDataAsync());
  }

  protected readonly UserStatus = UserStatus;
}
