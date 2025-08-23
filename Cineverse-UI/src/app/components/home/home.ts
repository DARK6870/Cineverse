import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit, signal } from '@angular/core';
import { RatingModule } from 'primeng/rating';
import { Button } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { ScreeningGraphQlService } from '../../services/graphQl/screening-graphql-service';
import { MovieGraphQlService } from '../../services/graphQl/movie-graphql-service';
import { Movie } from '../../common/models/movie';
import { LoadingService } from '../../services/loading/loading-service';
import { RouterLink } from '@angular/router';
import { Screening } from '../../common/models/screening';
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
    private screeningService: ScreeningGraphQlService,
    private movieService: MovieGraphQlService,
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
