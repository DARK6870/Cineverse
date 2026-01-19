import { Component, HostListener, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterHelper } from '../../shared/helpers/router.helper';

@Component({
  selector: 'cineverse-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: 'header.html',
  styleUrls: ['header.css']
})

export class Header {
  private routerHelper = inject(RouterHelper);
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
    this.routerHelper.navigate(path);
    this.closeMenu();
  }
}
