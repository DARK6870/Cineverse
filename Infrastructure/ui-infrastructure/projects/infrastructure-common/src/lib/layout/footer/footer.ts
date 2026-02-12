import { Component, inject } from '@angular/core';
import { RouterHelper } from '../../shared/helpers/router.helper';

@Component({
  selector: 'cineverse-footer',
  imports: [],
  templateUrl: './footer.html',
  styleUrl: './footer.css',
})
export class Footer {
  private routerHelper = inject(RouterHelper);

  protected navigate(path: string) {
    this.routerHelper.navigate(path);
  }
}
