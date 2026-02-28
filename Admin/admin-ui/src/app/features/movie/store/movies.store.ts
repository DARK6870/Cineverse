import { Injectable, computed, signal } from '@angular/core';
import { Movie, MovieFilters, MovieSort } from '../api/movie.graphql.types';
import { initialMoviesState, MoviesState } from './movies.state';

@Injectable({ providedIn: 'root' })
export class MoviesStore {
  // State
  private state = signal<MoviesState>(initialMoviesState);

  // Selectors
  readonly movies = computed(() => this.state().movies);
  readonly totalRecords = computed(() => this.state().totalRecords);
  readonly first = computed(() => this.state().first);
  readonly pageSize = computed(() => this.state().pageSize);
  readonly sort = computed(() => this.state().sort);
  readonly sortField = computed(() => this.state().sort.field);
  readonly sortOrder = computed(() => (this.state().sort.direction === 'ASC' ? 1 : -1));
  readonly filters = computed(() => this.state().filters);
  readonly searchTerm = computed(() => this.state().searchTerm);
  readonly genreFilterOptions = computed(() => this.state().genreFilterOptions);
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

  setQuery(
    first: number,
    pageSize: number,
    sort: MovieSort,
    filters: MovieFilters,
    searchTerm: string,
  ): void {
    this.state.update((s) => ({
      ...s,
      first,
      pageSize,
      sort,
      filters,
      searchTerm,
    }));
  }

  setPageData(movies: Movie[], totalRecords: number): void {
    this.state.update((s) => ({
      ...s,
      movies,
      totalRecords,
    }));
  }

  setGenreFilterOptions(values: string[]): void {
    this.state.update((s) => ({
      ...s,
      genreFilterOptions: values.filter((value) => !!value?.trim()),
    }));
  }
}
