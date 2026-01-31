import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MovieGraphqlService } from '../../api/movie.graphql.service';
import { Movie } from '../../api/movie.graphql.types';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Screening } from '../../../screening/api/screening.graphql.types';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Button } from 'primeng/button';
import { MovieDetailsCard } from '../../../../shared/components/movie-details-card/movie-details-card';
import { ToastService } from '@cineverse/infrastructure-common';

@Component({
  selector: 'app-movies-details',
  imports: [Button, RouterLink, MovieDetailsCard],
  standalone: true,
  templateUrl: 'movie-details.html',
  styleUrl: 'movie-details.css',
})
export class MovieDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private movieGraphqlService = inject(MovieGraphqlService);
  private screeningGraphqlService = inject(ScreeningGraphqlService);
  private destroyRef = inject(DestroyRef);
  private toastService = inject(ToastService);

  movie = signal<Movie | null>(null);
  screenings = signal<Screening[] | null>(null);

  // TODO: change parammap
  async ngOnInit() {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(async (paramMap) => {
        const movieId = paramMap.get('movieId');
        if (!movieId) return;

        const movie = await this.movieGraphqlService.getMovieById(movieId);

        const screenings = await this.screeningGraphqlService.getActiveScreeningsForMovie(movieId);

        if (!movie || !screenings) {
          this.toastService.error('Movie was not found');
          throw new Error('Movie was not found');
        }

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
