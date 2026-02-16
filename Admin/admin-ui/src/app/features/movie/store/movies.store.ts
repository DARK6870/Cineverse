import { Injectable, inject } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { from } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { MovieGraphqlService } from '../api/movie.graphql.service';
import { MoviesState, initialMoviesState } from './movies.state';

@Injectable()
export class MoviesStore extends ComponentStore<MoviesState> {
  private movieService = inject(MovieGraphqlService);

  constructor() {
    super(initialMoviesState);
  }

  readonly movies$ = this.select((state) => state.movies);
  readonly totalRecords$ = this.select((state) => state.totalRecords);
  readonly pageSize$ = this.select((state) => state.pageSize);
  readonly first$ = this.select((state) => state.first);

  readonly loadPage = this.effect<{ first: number; pageSize: number }>((params$) =>
    params$.pipe(
      tap(({ first, pageSize }) => {
        this.patchState({ first, pageSize });
      }),
      switchMap(({ first, pageSize }) =>
        from(this.movieService.getMoviesPage(first, pageSize)).pipe(
          tap((data) => {
            const items = data.items ?? [];
            this.patchState({
              movies: items,
              totalRecords: data.totalCount ?? items.length,
            });
          }),
        ),
      ),
    ),
  );

  refresh(): void {
    const { first, pageSize } = this.get();
    this.loadPage({ first, pageSize });
  }

}
