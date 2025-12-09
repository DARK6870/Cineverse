import { Component, inject } from '@angular/core';
import { LoadingService } from '../../../core/services/loading.service';

@Component({
  selector: 'app-loading-overlay',
  templateUrl: 'loading-overlay.html',
  styleUrl: 'loading-overlay.css'
})
export class LoadingOverlay {
  public loadingService = inject(LoadingService);
}

// TODO: fix fonts
