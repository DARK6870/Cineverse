import { Component, inject } from '@angular/core';
import { BlockActionsService, LoadingService } from '@cineverse/infrastructure-common';

@Component({
  selector: 'app-loading-overlay',
  templateUrl: 'loading-overlay.html',
  styleUrl: 'loading-overlay.css'
})
export class LoadingOverlay {
  public loadingService = inject(LoadingService);
  public blockingService = inject(BlockActionsService);
}
