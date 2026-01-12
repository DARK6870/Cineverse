import { Component, HostListener, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'cineverse-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: 'header.html',
  styleUrls: ['header.css']
})

export class Header {
  private router = inject(Router);
  isMenuOpen = false;

  toggleMenu(event: Event) {
    event.stopPropagation();
    this.isMenuOpen = !this.isMenuOpen;
  }

  closeMenu() {
    this.isMenuOpen = false;
  }

  @HostListener('document:click', ['$event'])
  onClick(event: Event) {
    if (!(event.target as Element).closest('.menu') &&
      !(event.target as Element).closest('.menu-icon')) {
      this.isMenuOpen = false;
    }
  }

  protected navigate(path: string) {
    const currentPath = window.location.pathname;
    const isOnIdentity = currentPath.startsWith('/identity');
    const targetIsIdentity = path.startsWith('/identity');

    if (isOnIdentity !== targetIsIdentity) {
      window.location.href = path;
    } else {
      this.router.navigate([path]).then();
    }

    this.closeMenu();
  }
}
