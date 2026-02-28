import { inject, Injectable } from '@angular/core';
import { MovieGraphqlService } from '../api/movie.graphql.service';
import { CreateMovieRequestInput, Movie, MovieFilters, MovieSort, UpdateMovieRequestInput } from '../api/movie.graphql.types';
import { MoviesStore } from './movies.store';

@Injectable({ providedIn: 'root' })
export class MoviesFacade {
  private store = inject(MoviesStore);
  private movieService = inject(MovieGraphqlService);

  readonly movies = this.store.movies;
  readonly totalRecords = this.store.totalRecords;
  readonly pageSize = this.store.pageSize;
  readonly first = this.store.first;
  readonly sort = this.store.sort;
  readonly sortField = this.store.sortField;
  readonly sortOrder = this.store.sortOrder;
  readonly filters = this.store.filters;
  readonly searchTerm = this.store.searchTerm;
  readonly genreFilterOptions = this.store.genreFilterOptions;
  readonly genreFilter = this.store.genreFilter;
  readonly availabilityFilter = this.store.availabilityFilter;

  loadPage(first: number, pageSize: number, sort?: MovieSort, filters?: MovieFilters, searchTerm?: string): void {
    void this.loadPageInternal(first, pageSize, sort, filters, searchTerm);
  }

  applyFilters(filters: MovieFilters): void {
    const currentSort = this.sort();
    const currentSearchTerm = this.searchTerm();
    void this.loadPageInternal(0, this.pageSize(), currentSort, filters, currentSearchTerm);
  }

  applySearch(searchTerm: string): void {
    void this.loadPageInternal(0, this.pageSize(), this.sort(), this.filters(), searchTerm.trim());
  }

  loadGenreFilterOptions(): void {
    void this.loadGenreFilterOptionsInternal();
  }

  getMovieById(id: string): Promise<Movie> {
    return this.movieService.getMovieById(id);
  }

  async createMovie(request: CreateMovieRequestInput): Promise<void> {
    await this.movieService.createMovie(request);
  }

  async updateMovie(request: UpdateMovieRequestInput): Promise<void> {
    await this.movieService.updateMovie(request);
  }

  async deleteMovie(id: string): Promise<void> {
    await this.movieService.deleteMovie(id);
  }

  refresh(): void {
    void this.loadPageInternal(this.first(), this.pageSize(), this.sort(), this.filters(), this.searchTerm());
  }

  private async loadPageInternal(
    first: number,
    pageSize: number,
    sort?: MovieSort,
    filters?: MovieFilters,
    searchTerm?: string,
  ): Promise<void> {
    const effectiveSort = sort ?? this.sort();
    const effectiveFilters = filters ?? this.filters();
    const effectiveSearchTerm = searchTerm ?? this.searchTerm();

    this.store.setQuery(first, pageSize, effectiveSort, effectiveFilters, effectiveSearchTerm);
    const data = await this.movieService.getMoviesPage(first, pageSize, effectiveSort, effectiveFilters, effectiveSearchTerm);
    this.store.setPageData(data.items ?? [], data.totalCount ?? 0);
  }

  private async loadGenreFilterOptionsInternal(): Promise<void> {
    const values = await this.movieService.getGenreDistinctFilterValues();
    this.store.setGenreFilterOptions(values);
  }
}
