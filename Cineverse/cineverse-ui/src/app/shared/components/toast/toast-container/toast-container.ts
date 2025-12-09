import { Component, inject } from '@angular/core';
import { ToastItem } from '../toast-item/toast-item';
import { ToastService } from '../../../../core/services/toast.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toast-container',
  imports: [
    ToastItem,
    CommonModule
  ],
  templateUrl: 'toast-container.html',
  styleUrl: 'toast-container.css'
})
export class ToastContainer {
  public toastService = inject(ToastService);
}
