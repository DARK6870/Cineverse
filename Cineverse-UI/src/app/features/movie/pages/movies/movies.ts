import { Component, OnInit, signal } from '@angular/core';
import { MovieGraphqlService } from '../../api/movie.graphql.service';
import { Movie } from '../../api/movie.graphql.types';
import { firstValueFrom } from 'rxjs';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { LoadingService } from '../../../../core/services/loading.service';
import { Button } from 'primeng/button';
import { RouterLink } from '@angular/router';
import { InputText } from 'primeng/inputtext';
import { InputIcon } from 'primeng/inputicon';
import { IconField } from 'primeng/iconfield';

// TODO: create a card component
@Component({
  selector: 'app-movies',
  imports: [
    Button,
    RouterLink,
    InputText,
    InputIcon,
    IconField
  ],
  templateUrl: 'movies.html',
  styleUrl: 'movies.css'
})
export class Movies implements OnInit {
  movies = signal<Movie[]>([]);

  constructor(
    private movieGraphqlService: MovieGraphqlService,
    private screeningGraphqlService: ScreeningGraphqlService,
    private loadingService: LoadingService
  ) { }

  async ngOnInit(){
    this.loadingService.show();

    const screeningMovieIds = await firstValueFrom(
      this.screeningGraphqlService.getScreeningMovieIds()
    );

    const data = await firstValueFrom(
      this.movieGraphqlService.getMoviesByIds(screeningMovieIds)
    );
    this.movies.set(data);

    this.loadingService.hide();
  }
}
