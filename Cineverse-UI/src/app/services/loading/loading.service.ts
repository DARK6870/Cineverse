import { Injectable, NgZone } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private _loading = new BehaviorSubject<boolean>(false);
  readonly loading$ = this._loading.asObservable();

  constructor(private zone: NgZone) {}

  show() {
    this.zone.run(() => {
      queueMicrotask(() => this._loading.next(true));
    });
  }

  hide() {
    this.zone.run(() => {
      queueMicrotask(() => this._loading.next(false));
    });
  }
}
