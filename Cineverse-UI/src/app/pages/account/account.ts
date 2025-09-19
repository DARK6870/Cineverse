import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication/authentication.service';

@Component({
  selector: 'app-account',
  imports: [],
  templateUrl: 'account.html',
  styleUrl: 'account.css'
})

export class Account implements OnInit{

  constructor(
    private authenticationService: AuthenticationService
  ) {
  }

  async ngOnInit() {
    this.authenticationService.requireRefreshToken();
  }
}
