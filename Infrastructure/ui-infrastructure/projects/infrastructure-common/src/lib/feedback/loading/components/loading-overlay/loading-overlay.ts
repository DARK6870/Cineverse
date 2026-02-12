import { Component, inject } from '@angular/core';
import { LoadingService } from '../../services/loading.service';

@Component({
  selector: 'cineverse-loading-overlay',
  templateUrl: 'loading-overlay.html',
  styleUrl: 'loading-overlay.css'
})
export class LoadingOverlay {
  public loadingService = inject(LoadingService);
}
