import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../../core/services/authentication.service';

@Component({
  selector: 'app-logout',
  imports: [],
  templateUrl: 'logout.html',
  styleUrl: 'logout.css'
})

export class Logout implements OnInit {

  constructor(private authenticationService: AuthenticationService) {
  }
    async ngOnInit() {
        await this.authenticationService.logoutUserAsync();
    }
}
