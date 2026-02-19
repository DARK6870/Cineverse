import { Injectable, inject, signal, computed } from '@angular/core';
import { MovieGraphqlService } from '../api/movie.graphql.service';
import { Movie, MovieFilters, MovieSort } from '../api/movie.graphql.types';

interface MoviesState {
  movies: Movie[];
  totalRecords: number;
  first: number;
  pageSize: number;
  sort: MovieSort;
  filters: MovieFilters;
}

const DEFAULT_SORT: MovieSort = {
  field: 'dateCreated',
  direction: 'DESC',
};

const initialState: MoviesState = {
  movies: [],
  totalRecords: 0,
  first: 0,
  pageSize: 25,
  sort: DEFAULT_SORT,
  filters: {},
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
  readonly sortField = computed(() => this.state().sort.field);
  readonly sortOrder = computed(() => (this.state().sort.direction === 'ASC' ? 1 : -1));
  readonly genreFilter = computed(() => {
    const genre = this.state().filters.genre;
    if (Array.isArray(genre)) {
      return genre;
    }

    return genre ? [genre] : [];
  });
  readonly availabilityFilter = computed(() => {
    const isAvailable = this.state().filters.isAvailable;

    if (Array.isArray(isAvailable)) {
      return isAvailable
        .map((value) => (value ? 'available' : 'unavailable'))
        .filter((value, index, values) => values.indexOf(value) === index);
    }

    if (typeof isAvailable !== 'boolean') {
      return [];
    }

    return [isAvailable ? 'available' : 'unavailable'];
  });

  async loadPage(first: number, pageSize: number, sort?: MovieSort, filters?: MovieFilters): Promise<void> {
    const effectiveSort = sort ?? this.state().sort;
    const effectiveFilters = filters ?? this.state().filters;
    this.state.update((s) => ({ ...s, first, pageSize, sort: effectiveSort, filters: effectiveFilters }));

    const data = await this.movieService.getMoviesPage(first, pageSize, effectiveSort, effectiveFilters);
    this.state.update((s) => ({
      ...s,
      movies: data.items ?? [],
      totalRecords: data.totalCount ?? 0,
    }));
  }

  async applyFilters(filters: MovieFilters): Promise<void> {
    const { pageSize, sort } = this.state();
    await this.loadPage(0, pageSize, sort, filters);
  }

  async refresh(): Promise<void> {
    const { first, pageSize, sort, filters } = this.state();
    await this.loadPage(first, pageSize, sort, filters);
  }
}
