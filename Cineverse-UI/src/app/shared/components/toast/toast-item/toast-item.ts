import { Component, Input } from '@angular/core';
import {Toast} from '../../../models/toast.model';
import {NgClass, NgSwitch} from '@angular/common';

@Component({
  selector: 'app-toast',
  imports: [
    NgClass
  ],
  templateUrl: 'toast-item.html',
  styleUrl: 'toast-item.css'
})
export class ToastItem {
  @Input() toast!: Toast;
}
