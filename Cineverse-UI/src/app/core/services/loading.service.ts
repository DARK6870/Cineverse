import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private _activeRequests = 0;
  readonly isLoading = signal(false);

  public show(): void {
    console.log(this._activeRequests)
    this._activeRequests++;
    this.isLoading.set(true);
  }

  public hide(): void {
    if (this._activeRequests <= 1) {
      this.reset();
    }

    this._activeRequests--;
  }

  public reset(){
    this._activeRequests = 0;
    this.isLoading.set(false);
  }
}
