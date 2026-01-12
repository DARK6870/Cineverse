import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'cineverse-footer',
  imports: [],
  templateUrl: './footer.html',
  styleUrl: './footer.css',
})
export class Footer {
  private router = inject(Router);

  protected navigate(path: string) {
    const currentPath = window.location.pathname;
    const isOnIdentity = currentPath.startsWith('/identity');
    const targetIsIdentity = path.startsWith('/identity');

    if (isOnIdentity !== targetIsIdentity) {
      window.location.href = path;
    } else {
      this.router.navigate([path]).then();
    }
  }
}
