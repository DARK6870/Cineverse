import { Component, Input } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RouterLink } from '@angular/router';
import { Movie } from '../../../features/movie/api/movie.graphql.types';
import { formatDate } from '../../utils/date-utils'

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

  protected readonly formatDate = formatDate;
}
