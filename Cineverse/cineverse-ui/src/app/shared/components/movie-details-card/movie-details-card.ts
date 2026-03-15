import { Component, inject, Input } from '@angular/core';
import { Movie } from '../../../features/movie/api/movie.graphql.types';
import { formatDate } from '@cineverse/infrastructure-common'
import { MarkdownComponent } from 'ngx-markdown';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-movie-details-card',
  imports: [MarkdownComponent],
  templateUrl: 'movie-details-card.html',
  styleUrl: 'movie-details-card.css',
})
export class MovieDetailsCard {
  @Input() set movie(value: Movie) {
    this._movie = value;
    this.trailerUrlSafe = value?.trailerUrl
      ? this.sanitizer.bypassSecurityTrustResourceUrl(value.trailerUrl)
      : null;
  }
  get movie(): Movie {
    return this._movie;
  }
  @Input() showTrailer: boolean = false;

  private sanitizer = inject(DomSanitizer);

  protected readonly formatDate = formatDate;

  protected trailerUrlSafe: SafeResourceUrl | null = null;

  private _movie!: Movie;
}
