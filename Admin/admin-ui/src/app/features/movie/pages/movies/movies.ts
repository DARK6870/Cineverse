import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonDirective } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { filter } from 'rxjs/operators';
import { MoviesFacade } from '../../store/movies.facade';

@Component({
  selector: 'app-movies',
  imports: [CommonModule, TableModule, ButtonDirective, RouterLink, TagModule],
  templateUrl: 'movies.html',
  styleUrl: 'movies.css',
  standalone: true,

})
export class Movies implements OnInit {
  protected facade = inject(MoviesFacade);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  tableHeight = 'calc(100vh - 300px)';

  ngOnInit() {
    this.route.queryParams.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(() => {
      this.facade.loadPage(0, 25);
    });

    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(() => {
        if (this.router.url.startsWith('/movies') && history.state?.refresh) {
          this.facade.refresh();
        }
      });
  }
}
