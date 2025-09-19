import { Component } from '@angular/core';
import { LoadingService } from '../../services/loading/loading.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-loading-overlay',
  imports: [
    AsyncPipe
  ],
  templateUrl: 'loading-overlay.html',
  styleUrl: 'loading-overlay.css'
})
export class LoadingOverlay {
  constructor(public loadingService: LoadingService) {}
}

// TODO: fix fonts
