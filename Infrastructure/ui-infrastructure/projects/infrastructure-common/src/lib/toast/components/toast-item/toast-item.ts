import { Component, Input } from '@angular/core';
import { NgClass } from '@angular/common';
import { Toast } from '../../models/toast.model';

@Component({
  selector: 'lib-toast',
  imports: [
    NgClass
  ],
  templateUrl: 'toast-item.html',
  styleUrl: 'toast-item.css'
})
export class ToastItem {
  @Input() toast!: Toast;
}
