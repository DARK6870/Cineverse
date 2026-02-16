import { Injectable, inject, signal, computed } from '@angular/core';
import { MovieGraphqlService } from '../api/movie.graphql.service';
import { Movie } from '../api/movie.graphql.types';

interface MoviesState {
  movies: Movie[];
  totalRecords: number;
  first: number;
  pageSize: number;
  loading: boolean;
}

const initialState: MoviesState = {
  movies: [],
  totalRecords: 0,
  first: 0,
  pageSize: 25,
  loading: false,
};

@Injectable({ providedIn: 'root' })
export class MoviesStore {
  private movieService = inject(MovieGraphqlService);

  // State
  private state = signal<MoviesState>(initialState);

  // Selectors
  readonly movies = computed(() => this.state().movies);
  readonly totalRecords = computed(() => this.state().totalRecords);
  readonly first = computed(() => this.state().first);
  readonly pageSize = computed(() => this.state().pageSize);
  readonly loading = computed(() => this.state().loading);

  async loadPage(first: number, pageSize: number): Promise<void> {
    this.state.update((s) => ({ ...s, first, pageSize, loading: true }));
    try {
      const data = await this.movieService.getMoviesPage(first, pageSize);
      this.state.update((s) => ({
        ...s,
        movies: data.items ?? [],
        totalRecords: data.totalCount ?? 0,
        loading: false,
      }));
    } catch (error) {
      this.state.update((s) => ({ ...s, loading: false }));
      throw error;
    }
  }

  async refresh(): Promise<void> {
    const { first, pageSize } = this.state();
    await this.loadPage(first, pageSize);
  }
}
