import { Component, inject } from '@angular/core';
import { BlockActionsService } from '../../services/block-actions.service';

@Component({
  selector: 'lib-blocking-overlay',
  imports: [],
  templateUrl: 'blocking-overlay.html',
  standalone: true,
  /*animations: [
    trigger('fade', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-out', style({ opacity: 1 }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0 }))
      ])
    ])
  ]*/

})
export class BlockingOverlay {
  protected service = inject(BlockActionsService);
}
