import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ButtonDirective } from 'primeng/button';

@Component({
  selector: 'app-sidebar',
  imports: [ButtonDirective, RouterLink, RouterLinkActive],
  templateUrl: 'sidebar.html',
  styleUrl: 'sidebar.css',
})
export class Sidebar {}
