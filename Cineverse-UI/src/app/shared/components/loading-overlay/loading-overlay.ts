import { Component } from '@angular/core';
import { LoadingService } from '../../../core/services/loading.service';

@Component({
  selector: 'app-loading-overlay',
  templateUrl: 'loading-overlay.html',
  styleUrl: 'loading-overlay.css'
})
export class LoadingOverlay {
  constructor(public loadingService: LoadingService) {}
}

// TODO: fix fonts
