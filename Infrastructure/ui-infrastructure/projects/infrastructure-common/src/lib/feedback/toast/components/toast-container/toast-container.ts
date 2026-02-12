import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast.service';
import { ToastItem } from '../toast-item/toast-item';

@Component({
  selector: 'cineverse-toast-container',
  imports: [
    ToastItem,
    CommonModule
  ],
  templateUrl: 'toast-container.html',
  styleUrl: 'toast-container.css',
  standalone: true
})
export class ToastContainer {
  public toastService = inject(ToastService);
}
