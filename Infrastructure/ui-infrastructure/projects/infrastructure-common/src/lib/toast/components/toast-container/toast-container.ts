import { Component, inject } from '@angular/core';
import { ToastItem } from '../toast-item/toast-item';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'lib-toast-container',
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
