import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'lib-header',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: 'header.html',
  styleUrls: ['header.css']
})

// TODO: add icons for mobile
export class Header {
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
}
