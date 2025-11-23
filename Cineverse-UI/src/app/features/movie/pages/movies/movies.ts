import { Component, inject, OnInit, signal } from '@angular/core';
import { MovieGraphqlService } from '../../api/movie.graphql.service';
import { Movie } from '../../api/movie.graphql.types';
import { firstValueFrom } from 'rxjs';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Button } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { InputIcon } from 'primeng/inputicon';
import { IconField } from 'primeng/iconfield';
import { MovieCard } from '../../../../shared/components/movie-card/movie-card';

@Component({
  selector: 'app-movies',
  imports: [
    Button,
    InputText,
    InputIcon,
    IconField,
    MovieCard
  ],
  standalone: true,
  templateUrl: 'movies.html',
  styleUrl: 'movies.css'
})
export class Movies implements OnInit {
  private movieGraphqlService = inject(MovieGraphqlService);
  private screeningGraphqlService = inject(ScreeningGraphqlService);

  movies = signal<Movie[]>([]);

  async ngOnInit(){
    const screeningMovieIds = await firstValueFrom(
      this.screeningGraphqlService.getScreeningMovieIds()
    );

    const data = await firstValueFrom(
      this.movieGraphqlService.getMoviesByIds(screeningMovieIds)
    );
    this.movies.set(data);
  }
}
