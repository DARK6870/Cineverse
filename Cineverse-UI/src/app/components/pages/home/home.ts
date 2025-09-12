import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit, signal } from '@angular/core';
import { RatingModule } from 'primeng/rating';
import { Button } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { ScreeningGraphqlService } from '../../../api/screening/screening.graphql.service';
import { MovieGraphqlService } from '../../../api/movie/movie.graphql.service';
import { Movie } from '../../../utils/types/api/movie';
import { LoadingService } from '../../../services/loading/loading.service';
import { RouterLink } from '@angular/router';
import { Screening } from '../../../utils/types/api/screening';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-home',
  imports: [
    RatingModule,
    Button,
    FormsModule,
    RouterLink,
  ],
  templateUrl: 'home.html',
  styleUrl: 'home.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class Home implements OnInit {
  movies = signal<Movie[]>([]);
  screenings = signal<Screening[]>([]);
  comingSoonMovies = signal<Movie[]>([]);

  constructor(
    private screeningService: ScreeningGraphqlService,
    private movieService: MovieGraphqlService,
    private loadingService: LoadingService
  ) {}
  ngOnInit(): void {
    this.loadingService.show();

    const today = new Date();
    const twoWeeksLater = new Date();
    twoWeeksLater.setDate(today.getDate() + 14);

    this.screeningService.getScreenings().pipe(
      switchMap(screeningsResult => {
        this.screenings.set(screeningsResult);

        const movieIds = [...new Set(this.screenings().map(screening => screening.movieId))];
        return this.movieService.getMovies(movieIds);
      })
    ).subscribe({
      next: (moviesResult) => {
        const comingSoon = moviesResult.filter(movie => {
          const movieScreenings = this.screenings().filter(screening =>
            screening.movieId === movie.id
          );

          if (movieScreenings.length === 0) return false;

          const nearestScreeningDate = movieScreenings
            .map(screening => new Date(screening.date))[0];

          return nearestScreeningDate >= twoWeeksLater;
        });
        this.comingSoonMovies.set(comingSoon);
        const comingSoonIds = new Set(comingSoon.map(m => m.id));

        this.movies.set(
          moviesResult.filter(m => !comingSoonIds.has(m.id))
        );

        this.loadingService.hide();
      }
    });
  }
}
