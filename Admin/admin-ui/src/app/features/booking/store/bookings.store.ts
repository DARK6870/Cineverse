import { Injectable, computed, signal } from '@angular/core';
import { Booking, BookingFilters, BookingReferenceOption, BookingSort } from '../api/booking.graphql.types';
import { BookingsState, initialBookingsState } from './bookings.state';

@Injectable({ providedIn: 'root' })
export class BookingsStore {
  private state = signal<BookingsState>(initialBookingsState);

  readonly bookings = computed(() => this.state().bookings);
  readonly totalRecords = computed(() => this.state().totalRecords);
  readonly first = computed(() => this.state().first);
  readonly pageSize = computed(() => this.state().pageSize);
  readonly sort = computed(() => this.state().sort);
  readonly sortField = computed(() => this.state().sort.field);
  readonly sortOrder = computed(() => (this.state().sort.direction === 'ASC' ? 1 : -1));
  readonly filters = computed(() => this.state().filters);
  readonly userFilterOptions = computed(() => this.state().userFilterOptions);
  readonly screeningFilterOptions = computed(() => this.state().screeningFilterOptions);
  readonly userLabels = computed(() => this.state().userLabels);
  readonly screeningLabels = computed(() => this.state().screeningLabels);

  readonly userFilter = computed(() => {
    const userId = this.state().filters.userId;
    if (Array.isArray(userId)) {
      return userId;
    }

    return userId ? [userId] : [];
  });

  readonly screeningFilter = computed(() => {
    const screeningId = this.state().filters.screeningId;
    if (Array.isArray(screeningId)) {
      return screeningId;
    }

    return screeningId ? [screeningId] : [];
  });

  setQuery(first: number, pageSize: number, sort: BookingSort, filters: BookingFilters): void {
    this.state.update((s) => ({
      ...s,
      first,
      pageSize,
      sort,
      filters,
    }));
  }

  setPageData(bookings: Booking[], totalRecords: number): void {
    this.state.update((s) => ({
      ...s,
      bookings,
      totalRecords,
    }));
  }

  setUserFilterOptions(options: BookingReferenceOption[]): void {
    this.state.update((s) => ({
      ...s,
      userFilterOptions: this.normalizeOptions(options),
    }));
  }

  setScreeningFilterOptions(options: BookingReferenceOption[]): void {
    this.state.update((s) => ({
      ...s,
      screeningFilterOptions: this.normalizeOptions(options),
    }));
  }

  mergeUserLabels(labels: Record<string, string>): void {
    this.state.update((s) => ({
      ...s,
      userLabels: {
        ...s.userLabels,
        ...labels,
      },
    }));
  }

  mergeScreeningLabels(labels: Record<string, string>): void {
    this.state.update((s) => ({
      ...s,
      screeningLabels: {
        ...s.screeningLabels,
        ...labels,
      },
    }));
  }

  private normalizeOptions(options: BookingReferenceOption[]): BookingReferenceOption[] {
    const deduplicatedById = new Map<string, BookingReferenceOption>();

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
