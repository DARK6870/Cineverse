import { Component, Input } from '@angular/core';
import { Toast } from '../../models/toast.model';
import { NgClass } from '@angular/common';

@Component({
  selector: 'cineverse-toast',
  imports: [
    NgClass
  ],
  templateUrl: 'toast-item.html',
  styleUrl: 'toast-item.css'
})
export class ToastItem {
  @Input() toast!: Toast;
}
