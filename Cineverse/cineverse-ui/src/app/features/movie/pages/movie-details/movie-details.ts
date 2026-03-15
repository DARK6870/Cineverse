import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MovieGraphqlService } from '../../api/movie.graphql.service';
import { Movie } from '../../api/movie.graphql.types';
import { Screening } from '../../../screening/api/screening.graphql.types';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Button } from 'primeng/button';
import { MovieDetailsCard } from '../../../../shared/components/movie-details-card/movie-details-card';

@Component({
  selector: 'app-movies-details',
  imports: [Button, RouterLink, MovieDetailsCard],
  standalone: true,
  templateUrl: 'movie-details.html',
  styleUrl: 'movie-details.css',
})
export class MovieDetails implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private movieGraphqlService = inject(MovieGraphqlService);
  private screeningGraphqlService = inject(ScreeningGraphqlService);

  movie = signal<Movie | null>(null);
  screenings = signal<Screening[] | null>(null);

  async ngOnInit() {
    const movieId = this.activatedRoute.snapshot.params['movieId'];
    const movie = await this.movieGraphqlService.getMovieById(movieId);
    const screenings = await this.screeningGraphqlService.getActiveScreeningsForMovie(movieId);

    this.movie.set(movie);
    this.screenings.set(screenings);
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
