import { Component, inject, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Header } from './layout/header/header';
import { Footer } from './layout/footer/footer';
import { filter } from 'rxjs';
import { LoadingOverlay } from './shared/components/loading-overlay/loading-overlay';
import { ToastContainer } from './shared/components/toast/toast-container/toast-container';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    Header,
    Footer,
    LoadingOverlay,
    ToastContainer,
  ],
  templateUrl: 'app.html',
  styleUrl: 'app.css'
})
export class App implements OnInit {
  private router = inject(Router);

  ngOnInit() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      window.scrollTo(0, 0);
    });
  }
}
