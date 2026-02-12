import { computed, Injectable, signal } from '@angular/core';

@Injectable({providedIn: 'root'})
export class BlockActionsService{
  private _blocked = signal(false);

  readonly isBlocked = computed(() => this._blocked());

  public block(): void {
    this._blocked.set(true);
  }

  public unblock(): void {
    this._blocked.set(false);
  }
}
