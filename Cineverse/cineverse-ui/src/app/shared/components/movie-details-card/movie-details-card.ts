import { Component, inject, Input } from '@angular/core';
import { Movie } from '../../../features/movie/api/movie.graphql.types';
import { formatDate } from '@cineverse/infrastructure-common'
import { MarkdownComponent } from 'ngx-markdown';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-movie-details-card',
  imports: [MarkdownComponent],
  templateUrl: 'movie-details-card.html',
  styleUrl: 'movie-details-card.css',
})
export class MovieDetailsCard {
  @Input() movie!: Movie;
  @Input() showTrailer: boolean = false;

  private sanitizer = inject(DomSanitizer);

  protected readonly formatDate = formatDate;

  protected getTrailerUrl() {
    return this.sanitizer.bypassSecurityTrustResourceUrl(this.movie.trailerUrl)
  }
}
