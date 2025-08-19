import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit, signal } from '@angular/core';
import { RatingModule } from 'primeng/rating';
import { Button } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { GraphqlService } from '../../services/graphql-service';
import { Movie } from '../../common/models/movie';
import {DatePipe, NgOptimizedImage} from '@angular/common';
import { LoadingService } from '../../services/loading-service';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [
    RatingModule,
    Button,
    FormsModule,
    DatePipe,
    RouterLink,
    NgOptimizedImage,
  ],
  templateUrl: 'home.html',
  styleUrl: 'home.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class Home implements OnInit {
  movies = signal<Movie[]>([]);

  constructor(
    private graphqlService: GraphqlService,
    private loadingService: LoadingService
  ) {}
  ngOnInit(): void {
    this.loadingService.show();
    this.graphqlService.getMovies().subscribe(result => {
      this.movies.set(result);
      this.loadingService.hide();
    });
  }
}
