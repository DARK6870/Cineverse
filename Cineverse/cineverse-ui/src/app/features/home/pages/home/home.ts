import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { RatingModule } from 'primeng/rating';
import { FormsModule } from '@angular/forms';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { MovieGraphqlService } from '../../../movie/api/movie.graphql.service';
import { RouterLink } from '@angular/router';
import { Movie } from '../../../movie/api/movie.graphql.types';
import { MovieCard } from '../../../../shared/components/movie-card/movie-card';

@Component({
  selector: 'app-home',
  imports: [RatingModule, FormsModule, RouterLink, MovieCard],
  standalone: true,
  templateUrl: 'home.html',
  styleUrl: 'home.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class Home implements OnInit {
  private screeningService = inject(ScreeningGraphqlService);
  private movieService = inject(MovieGraphqlService);

  movies = signal<Movie[]>([]);
  comingSoonMovies = signal<Movie[]>([]);

  async ngOnInit() {
    const comingSoon = await this.movieService.getComingSoonMovies();
    this.comingSoonMovies.set(comingSoon);

    const comingSoonIds = new Set(comingSoon.map((m) => m.id));

    const ids = await this.screeningService.getScreeningMovieIds();
    const movies = await this.movieService.getMoviesByIds(ids);

    this.movies.set(movies.filter((m) => !comingSoonIds.has(m.id)));
  }
}
