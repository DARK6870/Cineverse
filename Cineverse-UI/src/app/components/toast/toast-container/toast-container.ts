import { Component } from '@angular/core';
import { ToastItem } from '../toast-item/toast-item';
import { ToastService } from '../../../services/toast/toast.service';
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
  constructor(public toastService: ToastService) {}
}
