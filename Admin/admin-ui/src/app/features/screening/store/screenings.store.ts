import { Injectable, computed, signal } from '@angular/core';
import { Screening, ScreeningFilters, ScreeningReferenceOption, ScreeningSort } from '../api/screening.graphql.types';
import { initialScreeningsState, ScreeningsState } from './screenings.state';

@Injectable({ providedIn: 'root' })
export class ScreeningsStore {
  private state = signal<ScreeningsState>(initialScreeningsState);

  readonly screenings = computed(() => this.state().screenings);
  readonly totalRecords = computed(() => this.state().totalRecords);
  readonly first = computed(() => this.state().first);
  readonly pageSize = computed(() => this.state().pageSize);
  readonly sort = computed(() => this.state().sort);
  readonly sortField = computed(() => this.state().sort.field);
  readonly sortOrder = computed(() => (this.state().sort.direction === 'ASC' ? 1 : -1));
  readonly filters = computed(() => this.state().filters);
  readonly movieFilterOptions = computed(() => this.state().movieFilterOptions);
  readonly hallFilterOptions = computed(() => this.state().hallFilterOptions);
  readonly movieLabels = computed(() => this.state().movieLabels);
  readonly hallLabels = computed(() => this.state().hallLabels);

  readonly movieFilter = computed(() => {
    const movieId = this.state().filters.movieId;
    if (Array.isArray(movieId)) {
      return movieId;
    }

    return movieId ? [movieId] : [];
  });

  readonly hallFilter = computed(() => {
    const hallId = this.state().filters.hallId;
    if (Array.isArray(hallId)) {
      return hallId;
    }

    return hallId ? [hallId] : [];
  });

  readonly dateFilter = computed(() => this.state().filters.date ?? '');

  setQuery(first: number, pageSize: number, sort: ScreeningSort, filters: ScreeningFilters): void {
    this.state.update((s) => ({
      ...s,
      first,
      pageSize,
      sort,
      filters,
    }));
  }

  setPageData(screenings: Screening[], totalRecords: number): void {
    this.state.update((s) => ({
      ...s,
      screenings,
      totalRecords,
    }));
  }

  setMovieFilterOptions(options: ScreeningReferenceOption[]): void {
    this.state.update((s) => ({
      ...s,
      movieFilterOptions: this.normalizeOptions(options),
    }));
  }

  setHallFilterOptions(options: ScreeningReferenceOption[]): void {
    this.state.update((s) => ({
      ...s,
      hallFilterOptions: this.normalizeOptions(options),
    }));
  }

  mergeMovieLabels(labels: Record<string, string>): void {
    this.state.update((s) => ({
      ...s,
      movieLabels: {
        ...s.movieLabels,
        ...labels,
      },
    }));
  }

  mergeHallLabels(labels: Record<string, string>): void {
    this.state.update((s) => ({
      ...s,
      hallLabels: {
        ...s.hallLabels,
        ...labels,
      },
    }));
  }

  private normalizeOptions(options: ScreeningReferenceOption[]): ScreeningReferenceOption[] {
    const deduplicatedById = new Map<string, ScreeningReferenceOption>();

    for (const option of options) {
      const normalizedId = option.id?.trim();
      const normalizedLabel = option.label?.trim();
      if (!normalizedId || !normalizedLabel) {
        continue;
      }

      deduplicatedById.set(normalizedId, { id: normalizedId, label: normalizedLabel });
    }

    return [...deduplicatedById.values()].sort((a, b) => a.label.localeCompare(b.label));
  }
}
