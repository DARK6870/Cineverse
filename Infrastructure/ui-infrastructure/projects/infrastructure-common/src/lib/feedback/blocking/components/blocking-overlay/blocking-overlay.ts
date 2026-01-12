import { Component, inject, ViewEncapsulation } from '@angular/core';
import { BlockActionsService } from '../../services/block-actions.service';

@Component({
  selector: 'cineverse-blocking-overlay',
  imports: [],
  templateUrl: 'blocking-overlay.html',
  styleUrl: 'blocking-overlay.css',
  standalone: true
})
export class BlockingOverlay {
  protected service = inject(BlockActionsService);
}
