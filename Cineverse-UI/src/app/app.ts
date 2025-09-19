import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Header } from './components/header/header';
import { Footer } from './components/footer/footer';
import { filter } from 'rxjs';
import { LoadingOverlay } from './components/loading-overlay/loading-overlay';
import { ToastContainer } from './components/toast/toast-container/toast-container';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    Header,
    Footer,
    LoadingOverlay,
    ToastContainer
  ],
  templateUrl: 'app.html',
  styleUrl: 'app.css'
})
export class App implements OnInit {
  constructor(private router: Router) {}

  ngOnInit() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      window.scrollTo(0, 0);
    });
  }
}
