import { inject, Injectable } from '@angular/core';
import { MovieGraphqlService } from '../../movie/api/movie.graphql.service';
import { HallGraphqlService } from '../../hall/api/hall.graphql.service';
import {
  CreateScreeningRequestInput,
  Screening,
  ScreeningFilters,
  ScreeningReferenceOption,
  ScreeningSort,
  UpdateScreeningRequestInput,
} from '../api/screening.graphql.types';
import { ScreeningsStore } from './screenings.store';
import { ScreeningGraphqlService } from '../api/screening.graphql.service';

@Injectable({ providedIn: 'root' })
export class ScreeningsFacade {
  private static readonly MAX_PAGE_SIZE = 100;

  private store = inject(ScreeningsStore);
  private screeningService = inject(ScreeningGraphqlService);
  private movieService = inject(MovieGraphqlService);
  private hallService = inject(HallGraphqlService);

  readonly screenings = this.store.screenings;
  readonly totalRecords = this.store.totalRecords;
  readonly pageSize = this.store.pageSize;
  readonly first = this.store.first;
  readonly sort = this.store.sort;
  readonly sortField = this.store.sortField;
  readonly sortOrder = this.store.sortOrder;
  readonly filters = this.store.filters;
  readonly movieFilterOptions = this.store.movieFilterOptions;
  readonly hallFilterOptions = this.store.hallFilterOptions;
  readonly movieFilter = this.store.movieFilter;
  readonly hallFilter = this.store.hallFilter;
  readonly dateFilter = this.store.dateFilter;

  loadPage(first: number, pageSize: number, sort?: ScreeningSort, filters?: ScreeningFilters): void {
    void this.loadPageInternal(first, pageSize, sort, filters);
  }

  applyFilters(filters: ScreeningFilters): void {
    void this.loadPageInternal(0, this.pageSize(), this.sort(), filters);
  }

  loadMovieFilterOptions(): void {
    void this.loadMovieFilterOptionsInternal();
  }

  loadHallFilterOptions(): void {
    void this.loadHallFilterOptionsInternal();
  }

  ensureReferenceOptionsLoaded(): void {
    if (this.movieFilterOptions().length === 0) {
      this.loadMovieFilterOptions();
    }

    if (this.hallFilterOptions().length === 0) {
      this.loadHallFilterOptions();
    }
  }

  getMovieLabel(movieId: string): string {
    return this.store.movieLabels()[movieId] ?? movieId;
  }

  getHallLabel(hallId: string): string {
    return this.store.hallLabels()[hallId] ?? hallId;
  }

  async getScreeningById(id: string): Promise<Screening> {
    const screening = await this.screeningService.getScreeningById(id);
    await this.resolveLabels([screening]);
    return screening;
  }

  async createScreening(request: CreateScreeningRequestInput): Promise<void> {
    await this.screeningService.createScreening(request);
  }

  async updateScreening(request: UpdateScreeningRequestInput): Promise<void> {
    await this.screeningService.updateScreening(request);
  }

  async deleteScreening(id: string): Promise<void> {
    await this.screeningService.deleteScreening(id);
  }

  refresh(): void {
    void this.loadPageInternal(this.first(), this.pageSize(), this.sort(), this.filters());
  }

  private async loadPageInternal(
    first: number,
    pageSize: number,
    sort?: ScreeningSort,
    filters?: ScreeningFilters,
  ): Promise<void> {
    const effectiveSort = sort ?? this.sort();
    const effectiveFilters = filters ?? this.filters();

    this.store.setQuery(first, pageSize, effectiveSort, effectiveFilters);

    const data = await this.screeningService.getScreeningsPage(first, pageSize, effectiveSort, effectiveFilters);
    const items = data.items ?? [];

    this.store.setPageData(items, data.totalCount ?? 0);
    await this.resolveLabels(items);
  }

  private async loadMovieFilterOptionsInternal(): Promise<void> {
    const page = await this.movieService.getMoviesPage(
      0,
      ScreeningsFacade.MAX_PAGE_SIZE,
      { field: 'title', direction: 'ASC' },
      {},
      '',
    );

    const options: ScreeningReferenceOption[] = (page.items ?? []).map((movie) => ({
      id: movie.id,
      label: movie.title,
    }));

    this.store.setMovieFilterOptions(options);
    this.store.mergeMovieLabels(
      Object.fromEntries(options.map((option) => [option.id, option.label])),
    );
  }

  private async loadHallFilterOptionsInternal(): Promise<void> {
    const page = await this.hallService.getHallsPage(0, ScreeningsFacade.MAX_PAGE_SIZE, { field: 'name', direction: 'ASC' });

    const options: ScreeningReferenceOption[] = (page.items ?? []).map((hall) => ({
      id: hall.id,
      label: hall.name,
    }));

    this.store.setHallFilterOptions(options);
    this.store.mergeHallLabels(
      Object.fromEntries(options.map((option) => [option.id, option.label])),
    );
  }

  private async resolveLabels(screenings: Screening[]): Promise<void> {
    await Promise.all([
      this.resolveMovieLabels(screenings),
      this.resolveHallLabels(screenings),
    ]);
  }

  private async resolveMovieLabels(screenings: Screening[]): Promise<void> {
    const existingLabels = this.store.movieLabels();
    const missingMovieIds = screenings
      .map((screening) => screening.movieId)
      .filter((movieId, index, ids) => !!movieId && ids.indexOf(movieId) === index && !existingLabels[movieId]);

    if (missingMovieIds.length === 0) {
      return;
    }

    const movies = await this.movieService.getMoviesByIds(missingMovieIds);
    const labels: Record<string, string> = {};
    for (const movie of movies) {
      if (movie.id && movie.title) {
        labels[movie.id] = movie.title;
      }
    }

    this.store.mergeMovieLabels(labels);
  }

  private async resolveHallLabels(screenings: Screening[]): Promise<void> {
    const existingLabels = this.store.hallLabels();
    const missingHallIds = screenings
      .map((screening) => screening.hallId)
      .filter((hallId, index, ids) => !!hallId && ids.indexOf(hallId) === index && !existingLabels[hallId]);

    if (missingHallIds.length === 0) {
      return;
    }

    const hallRequests = missingHallIds.map((hallId) => this.hallService.getHallById(hallId));
    const hallResults = await Promise.allSettled(hallRequests);

    const labels: Record<string, string> = {};
    for (const hallResult of hallResults) {
      if (hallResult.status === 'fulfilled' && hallResult.value.id && hallResult.value.name) {
        labels[hallResult.value.id] = hallResult.value.name;
      }
    }

    this.store.mergeHallLabels(labels);
  }
}
