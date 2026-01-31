import { Component, inject, OnInit, signal } from '@angular/core';
import { MovieGraphqlService } from '../../api/movie.graphql.service';
import { Movie } from '../../api/movie.graphql.types';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { MovieCard } from '../../../../shared/components/movie-card/movie-card';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-movies',
  imports: [MovieCard, ReactiveFormsModule],
  standalone: true,
  templateUrl: 'movies.html',
  styleUrl: 'movies.css',
})
export class Movies implements OnInit {
  private movieGraphqlService = inject(MovieGraphqlService);
  private screeningGraphqlService = inject(ScreeningGraphqlService);

  movies = signal<Movie[]>([]);

  async ngOnInit() {
    const screeningMovieIds = await this.screeningGraphqlService.getScreeningMovieIds();
    const movies = await this.movieGraphqlService.getMoviesByIds(screeningMovieIds);

    this.movies.set(movies);
  }
}
