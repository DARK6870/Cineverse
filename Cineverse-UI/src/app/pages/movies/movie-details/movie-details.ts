import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieGraphqlService } from '../../../api/movie/movie.graphql.service';
import {firstValueFrom, lastValueFrom} from 'rxjs';
import { Movie } from '../../../api/movie/movie.graphql.types';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DomSanitizer } from '@angular/platform-browser';
import { ToastService } from '../../../services/toast/toast.service';
import { MarkdownComponent } from 'ngx-markdown';
import { Screening } from '../../../api/screening/screening.graphql.types';
import { ScreeningGraphqlService } from '../../../api/screening/screening.graphql.service';

@Component({
  selector: 'app-movies-details',
  imports: [
    MarkdownComponent
  ],
  templateUrl: 'movie-details.html',
  styleUrl: 'movie-details.css'
})
export class MovieDetails implements OnInit {
  movie = signal<Movie | null>(null);
  safeTrailerUrl = signal<string>("");

  screenings = signal<Screening[] | null>(null);

  constructor(
    private route: ActivatedRoute,
    private movieGraphqlService: MovieGraphqlService,
    private screeningGraphqlService: ScreeningGraphqlService,
    private destroyRef: DestroyRef,
    private sanitizer: DomSanitizer,
    private toastService: ToastService
  ) {

  }
    async ngOnInit() {
      this.route.paramMap
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe(async paramMap => {
          const movieId = paramMap.get('movieId');
          if (!movieId) return;

          // Get Movie
          const movie = await firstValueFrom(
            this.movieGraphqlService.getMovieById(movieId)
          );

          if (movie === null){
            this.toastService.error('Movie was not found');
            return;
          }

          this.safeTrailerUrl.set(
            <string>this.sanitizer.bypassSecurityTrustResourceUrl(movie.trailerUrl)
          );

          // Get Screenings
          const screenings = await firstValueFrom(
            this.screeningGraphqlService.getActiveScreeningsForMovie(movieId)
          );

          this.movie.set(movie);
          this.screenings.set(screenings);
        });
    }

  getDayName(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString(undefined, { weekday: 'long' });
  }
}
