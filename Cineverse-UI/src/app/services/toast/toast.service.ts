import { Toast, ToastType } from '../../utils/models/toast.model';
import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  private _toasts = signal<Toast[]>([]);
  public toasts$ = this._toasts;

  private show(message: string, type: ToastType = 'info'){
    const id = Date.now();
    const toast: Toast = {
      id,
      message,
      type
    };

    this._toasts.update(list => [...list, toast]);
    setTimeout(() => this.remove(id), 3000);
  }

  success(message: string) {
    this.show(message, 'success');
  }

  error(message: string) {
    this.show(message, 'error');
  }

  info(message: string) {
    this.show(message, 'info');
  }

  warning(message: string) {
    this.show(message, 'warning');
  }

  remove(id: number) {
    this._toasts.update(list =>
      list.map(t => t.id === id ? { ...t, hiding: true } : t)
    );

    setTimeout(() => {
      this._toasts.update(list => list.filter(t => t.id !== id));
    }, 300);
  }
}
