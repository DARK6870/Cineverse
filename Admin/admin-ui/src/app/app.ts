import { Component, inject, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { ToastContainer } from '@cineverse/infrastructure-common';
import { filter } from 'rxjs';
import { Sidebar } from './layout/sidebar/sidebar';
import { LoadingOverlay } from './layout/loading-overlay/loading-overlay';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastContainer, LoadingOverlay, Sidebar],
  templateUrl: 'app.html'
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
