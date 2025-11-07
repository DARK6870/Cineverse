import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit, signal } from '@angular/core';
import { RatingModule } from 'primeng/rating';
import { FormsModule } from '@angular/forms';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { MovieGraphqlService } from '../../../movie/api/movie.graphql.service';
import { RouterLink } from '@angular/router';
import { firstValueFrom, switchMap } from 'rxjs';
import { Movie } from '../../../movie/api/movie.graphql.types';
import { MovieCard } from '../../../../shared/components/movie-card/movie-card';

@Component({
  selector: 'app-home',
  imports: [
    RatingModule,
    FormsModule,
    RouterLink,
    MovieCard,
  ],
  standalone: true,
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
  ) {

  }

  async ngOnInit() {
    const today = new Date();
    const twoWeeksLater = new Date();
    twoWeeksLater.setDate(today.getDate() + 14);

    const movies$ = this.screeningService.getScreeningMovieIds().pipe(
      switchMap(ids => this.movieService.getMoviesByIds(ids))
    );

    const comingSoon = await firstValueFrom(this.movieService.getComingSoonMovies());
    this.comingSoonMovies.set(comingSoon);

    const comingSoonIds = new Set(comingSoon.map(m => m.id));
    const movies = await firstValueFrom(movies$);
    this.movies.set((await firstValueFrom(movies$)).filter(m => !comingSoonIds.has(m.id)));
  }
}
