import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MovieGraphqlService } from '../../api/movie.graphql.service';
import { firstValueFrom } from 'rxjs';
import { Movie } from '../../api/movie.graphql.types';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DomSanitizer } from '@angular/platform-browser';
import { ToastService } from '../../../../core/services/toast.service';
import { MarkdownComponent } from 'ngx-markdown';
import { Screening } from '../../../screening/api/screening.graphql.types';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Button } from 'primeng/button';

@Component({
  selector: 'app-movies-details',
  imports: [MarkdownComponent, Button, RouterLink],
  standalone: true,
  templateUrl: 'movie-details.html',
  styleUrl: 'movie-details.css',
})
export class MovieDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private movieGraphqlService = inject(MovieGraphqlService);
  private screeningGraphqlService = inject(ScreeningGraphqlService);
  private destroyRef = inject(DestroyRef);
  private sanitizer = inject(DomSanitizer);
  private toastService = inject(ToastService);

  movie = signal<Movie | null>(null);
  safeTrailerUrl = signal<string>('');

  screenings = signal<Screening[] | null>(null);

  async ngOnInit() {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(async (paramMap) => {
        const movieId = paramMap.get('movieId');
        if (!movieId) return;

        // Get Movie
        const movie = await firstValueFrom(
          this.movieGraphqlService.getMovieById(movieId),
        );

        if (movie === null) {
          this.toastService.error('Movie was not found');
          return;
        }

        this.safeTrailerUrl.set(
          <string>(
            this.sanitizer.bypassSecurityTrustResourceUrl(movie.trailerUrl)
          ),
        );

        // Get Screenings
        const screenings = await firstValueFrom(
          this.screeningGraphqlService.getActiveScreeningsForMovie(movieId),
        );

        this.movie.set(movie);
        this.screenings.set(screenings);
      });
  }

  getDayName(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString(undefined, { weekday: 'long' });
  }

  getFriendlyDate(dateString: string): string {
    const date = new Date(dateString);
    const options: Intl.DateTimeFormatOptions = {
      day: 'numeric',
      month: 'long',
    };
    return date.toLocaleDateString('en-GB', options);
  }
}
