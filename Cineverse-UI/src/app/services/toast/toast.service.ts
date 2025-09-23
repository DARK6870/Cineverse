import { Toast, ToastType } from '../../utils/models/toast.model';
import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  private _queue : Toast[] = [];
  private _toasts = signal<Toast[]>([]);
  public toasts$ = this._toasts;

  private show(message: string, type: ToastType = 'info'){
    const id = Date.now();
    const toast: Toast = {
      id,
      message,
      type
    };

    this._queue.push(toast);
    if (this._toasts().length === 0) {
      this.displayNext();
    }
  }

  private displayNext() {
    if (this._queue.length === 0) return;

    const toast = this._queue.shift()!;
    this._toasts.set([toast]);

    setTimeout(() => this.remove(toast.id), 3000);
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
      this._toasts.set([]);
      this.displayNext();
    }, 300);
  }
}
