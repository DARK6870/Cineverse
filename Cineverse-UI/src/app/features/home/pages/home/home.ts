import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit, signal } from '@angular/core';
import { RatingModule } from 'primeng/rating';
import { Button } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { MovieGraphqlService } from '../../../movie/api/movie.graphql.service';
import { LoadingService } from '../../../../core/services/loading.service';
import { RouterLink } from '@angular/router';
import { switchMap } from 'rxjs';
import { Movie } from '../../../movie/api/movie.graphql.types';

// TODO: create card component
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

    this.screeningService.getScreeningMovieIds().pipe(
      switchMap(screeningsResult => {
        return this.movieService.getMoviesByIds(screeningsResult);
      })
    ).subscribe({
      next: (moviesResult) => {
        const comingSoon = moviesResult.filter(movie => new Date(movie.releaseDate) > twoWeeksLater);

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
