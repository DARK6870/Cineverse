import { Component, Input } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RouterLink } from '@angular/router';
import { Movie } from '../../../features/movie/api/movie.graphql.types';

@Component({
  selector: 'app-movie-card',
  standalone: true,
  imports: [ButtonModule, RouterLink],
  templateUrl: 'movie-card.html',
  styleUrls: ['movie-card.css']
})
export class MovieCard {
  @Input() movie!: Movie;
  @Input() isShowingNow = false;

  get formattedReleaseDate(): string {
    if (!this.movie?.releaseDate) return '';
    const date = new Date(this.movie.releaseDate);
    return date.toLocaleDateString('en-US', {
      day: 'numeric',
      month: 'long',
    });
  }
}
