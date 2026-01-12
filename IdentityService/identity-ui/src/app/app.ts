import { Component, inject, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { BlockingOverlay, Footer, Header, LoadingOverlay, ToastContainer } from '@cineverse/infrastructure-common';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, ToastContainer, BlockingOverlay, LoadingOverlay, Footer, BlockingOverlay],
  templateUrl: 'app.html',
  styleUrl: 'app.css'
})
export class App implements OnInit {
  private router = inject(Router);

  ngOnInit() {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        window.scrollTo(0, 0);
      });
  }
}
