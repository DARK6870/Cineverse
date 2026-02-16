import { Component, inject, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { BlockingOverlay, LoadingOverlay, ToastContainer } from '@cineverse/infrastructure-common';
import { filter } from 'rxjs';
import { Sidebar } from './layout/sidebar/sidebar';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastContainer, BlockingOverlay, LoadingOverlay, Sidebar],
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
