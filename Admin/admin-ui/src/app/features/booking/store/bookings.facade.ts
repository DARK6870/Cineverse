import { inject, Injectable } from '@angular/core';
import { User, UserFilters } from '../../user/api/user.graphql.types';
import { UserGraphqlService } from '../../user/api/user.graphql.service';
import { HallGraphqlService } from '../../hall/api/hall.graphql.service';
import { MovieGraphqlService } from '../../movie/api/movie.graphql.service';
import { ScreeningGraphqlService } from '../../screening/api/screening.graphql.service';
import { Screening } from '../../screening/api/screening.graphql.types';
import { Booking, BookingFilters, BookingReferenceOption, BookingSort } from '../api/booking.graphql.types';
import { BookingGraphqlService } from '../api/booking.graphql.service';
import { BookingsStore } from './bookings.store';

@Injectable({ providedIn: 'root' })
export class BookingsFacade {
  private static readonly MAX_PAGE_SIZE = 100;

  private store = inject(BookingsStore);
  private bookingService = inject(BookingGraphqlService);
  private userService = inject(UserGraphqlService);
  private screeningService = inject(ScreeningGraphqlService);
  private movieService = inject(MovieGraphqlService);
  private hallService = inject(HallGraphqlService);

  readonly bookings = this.store.bookings;
  readonly totalRecords = this.store.totalRecords;
  readonly pageSize = this.store.pageSize;
  readonly first = this.store.first;
  readonly sort = this.store.sort;
  readonly sortField = this.store.sortField;
  readonly sortOrder = this.store.sortOrder;
  readonly filters = this.store.filters;
  readonly userFilterOptions = this.store.userFilterOptions;
  readonly screeningFilterOptions = this.store.screeningFilterOptions;
  readonly userFilter = this.store.userFilter;
  readonly screeningFilter = this.store.screeningFilter;

  loadPage(first: number, pageSize: number, sort?: BookingSort, filters?: BookingFilters): void {
    void this.loadPageInternal(first, pageSize, sort, filters);
  }

  applyFilters(filters: BookingFilters): void {
    void this.loadPageInternal(0, this.pageSize(), this.sort(), filters);
  }

  loadUserFilterOptions(): void {
    void this.loadUserFilterOptionsInternal();
  }

  loadScreeningFilterOptions(): void {
    void this.loadScreeningFilterOptionsInternal();
  }

  ensureReferenceOptionsLoaded(): void {
    if (this.userFilterOptions().length === 0) {
      this.loadUserFilterOptions();
    }

    if (this.screeningFilterOptions().length === 0) {
      this.loadScreeningFilterOptions();
    }
  }

  getUserLabel(userId: string): string {
    return this.store.userLabels()[userId] ?? userId;
  }

  getScreeningLabel(screeningId: string): string {
    return this.store.screeningLabels()[screeningId] ?? screeningId;
  }

  refresh(): void {
    void this.loadPageInternal(this.first(), this.pageSize(), this.sort(), this.filters());
  }

  private async loadPageInternal(
    first: number,
    pageSize: number,
    sort?: BookingSort,
    filters?: BookingFilters,
  ): Promise<void> {
    const effectiveSort = sort ?? this.sort();
    const effectiveFilters = filters ?? this.filters();

    this.store.setQuery(first, pageSize, effectiveSort, effectiveFilters);

    const data = await this.bookingService.getBookingsPage(first, pageSize, effectiveSort, effectiveFilters);
    const items = data.items ?? [];

    this.store.setPageData(items, data.totalCount ?? 0);
    await Promise.all([
      this.resolveUserLabels(items),
      this.resolveScreeningLabels(items),
    ]);
  }

  private async loadUserFilterOptionsInternal(): Promise<void> {
    const usersFilter: UserFilters = {};
    const page = await this.userService.getUsersPage(
      0,
      BookingsFacade.MAX_PAGE_SIZE,
      { field: 'email', direction: 'ASC' },
      usersFilter,
      '',
    );

    const options: BookingReferenceOption[] = (page.items ?? []).map((user) => ({
      id: user.id,
      label: this.buildUserLabel(user),
    }));

    this.store.setUserFilterOptions(options);
    this.store.mergeUserLabels(Object.fromEntries(options.map((option) => [option.id, option.label])));
  }

  private async loadScreeningFilterOptionsInternal(): Promise<void> {
    const screeningsPage = await this.screeningService.getScreeningsPage(
      0,
      BookingsFacade.MAX_PAGE_SIZE,
      { field: 'date', direction: 'DESC' },
      {},
    );

    const screenings = screeningsPage.items ?? [];
    const labels = await this.buildScreeningLabels(screenings);

    const options: BookingReferenceOption[] = screenings
      .map((screening) => ({
        id: screening.id,
        label: labels[screening.id] ?? screening.id,
      }));

    this.store.setScreeningFilterOptions(options);
    this.store.mergeScreeningLabels(Object.fromEntries(options.map((option) => [option.id, option.label])));
  }

  private async resolveUserLabels(bookings: Booking[]): Promise<void> {
    const existingLabels = this.store.userLabels();
    const missingUserIds = bookings
      .map((booking) => booking.userId)
      .filter((userId, index, ids) => !!userId && ids.indexOf(userId) === index && !existingLabels[userId]);

    if (missingUserIds.length === 0) {
      return;
    }

    const userRequests = missingUserIds.map((userId) => this.userService.getUserById(userId));
    const userResults = await Promise.allSettled(userRequests);

    const labels: Record<string, string> = {};
    for (const userResult of userResults) {
      if (userResult.status === 'fulfilled' && userResult.value.id) {
        labels[userResult.value.id] = this.buildUserLabel(userResult.value);
      }
    }

    this.store.mergeUserLabels(labels);
  }

  private async resolveScreeningLabels(bookings: Booking[]): Promise<void> {
    const existingLabels = this.store.screeningLabels();
    const missingScreeningIds = bookings
      .map((booking) => booking.screeningId)
      .filter((screeningId, index, ids) => !!screeningId && ids.indexOf(screeningId) === index && !existingLabels[screeningId]);

    if (missingScreeningIds.length === 0) {
      return;
    }

    const screeningRequests = missingScreeningIds.map((screeningId) => this.screeningService.getScreeningById(screeningId));
    const screeningResults = await Promise.allSettled(screeningRequests);
    const screenings = screeningResults
      .filter((screeningResult): screeningResult is PromiseFulfilledResult<Screening> => screeningResult.status === 'fulfilled')
      .map((screeningResult) => screeningResult.value);

    if (screenings.length === 0) {
      return;
    }

    const labels = await this.buildScreeningLabels(screenings);
    this.store.mergeScreeningLabels(labels);
  }

  private async buildScreeningLabels(screenings: Screening[]): Promise<Record<string, string>> {
    const movieIds = screenings
      .map((screening) => screening.movieId)
      .filter((movieId, index, ids) => !!movieId && ids.indexOf(movieId) === index);

    const hallIds = screenings
      .map((screening) => screening.hallId)
      .filter((hallId, index, ids) => !!hallId && ids.indexOf(hallId) === index);

    const [movies, hallResults] = await Promise.all([
      movieIds.length > 0 ? this.movieService.getMoviesByIds(movieIds) : Promise.resolve([]),
      Promise.allSettled(hallIds.map((hallId) => this.hallService.getHallById(hallId))),
    ]);

    const movieLabels = new Map<string, string>();
    for (const movie of movies) {
      if (movie.id && movie.title) {
        movieLabels.set(movie.id, movie.title);
      }
    }

    const hallLabels = new Map<string, string>();
    for (const hallResult of hallResults) {
      if (hallResult.status === 'fulfilled' && hallResult.value.id && hallResult.value.name) {
        hallLabels.set(hallResult.value.id, hallResult.value.name);
      }
    }

    const labels: Record<string, string> = {};
    for (const screening of screenings) {
      labels[screening.id] = this.formatScreeningLabel(
        movieLabels.get(screening.movieId) ?? screening.movieId,
        hallLabels.get(screening.hallId) ?? screening.hallId,
        screening.date,
        screening.startTime,
      );
    }

    return labels;
  }

  private buildUserLabel(user: User): string {
    const fullName = `${user.firstName ?? ''} ${user.lastName ?? ''}`.trim();
    if (fullName && user.email) {
      return `${fullName} (${user.email})`;
    }

    return user.email || fullName || user.id;
  }

  private formatScreeningLabel(movieLabel: string, hallLabel: string, date: string, startTime: string): string {
    const normalizedTime = (startTime ?? '').slice(0, 5);
    const timeLabel = normalizedTime || '--:--';
    return `${movieLabel} | ${hallLabel} | ${date} ${timeLabel}`;
  }
}
