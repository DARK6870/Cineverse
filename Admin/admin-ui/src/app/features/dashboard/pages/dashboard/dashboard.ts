import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { MovieGraphqlService } from '../../../movie/api/movie.graphql.service';
import { UserGraphqlService } from '../../../user/api/user.graphql.service';
import { HallGraphqlService } from '../../../hall/api/hall.graphql.service';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { BookingGraphqlService } from '../../../booking/api/booking.graphql.service';
import { Booking } from '../../../booking/api/booking.graphql.types';
import { Screening } from '../../../screening/api/screening.graphql.types';

interface DashboardTotals {
  movies: number;
  users: number;
  halls: number;
  screenings: number;
  bookings: number;
}

interface TrendPoint {
  label: string;
  value: number;
}

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: 'dashboard.html',
  styleUrl: 'dashboard.css',
})
export class Dashboard {
  private static readonly ANALYTICS_LIMIT = 2000;

  private movieService = inject(MovieGraphqlService);
  private userService = inject(UserGraphqlService);
  private hallService = inject(HallGraphqlService);
  private screeningService = inject(ScreeningGraphqlService);
  private bookingService = inject(BookingGraphqlService);

  readonly loading = signal(true);
  readonly analyticsNotice = signal<string | null>(null);

  readonly totals = signal<DashboardTotals>({
    movies: 0,
    users: 0,
    halls: 0,
    screenings: 0,
    bookings: 0,
  });

  readonly screeningsThisWeek = signal(0);
  readonly bookingsThisWeek = signal(0);
  readonly revenueThisWeek = signal(0);
  readonly bookingsThisMonth = signal(0);
  readonly revenueThisMonth = signal(0);

  readonly bookingsWeeklyTrend = signal<TrendPoint[]>([]);
  readonly screeningsMonthlyTrend = signal<TrendPoint[]>([]);

  readonly bookingTrendMax = computed(() =>
    this.bookingsWeeklyTrend().reduce((max, point) => Math.max(max, point.value), 0),
  );
  readonly screeningTrendMax = computed(() =>
    this.screeningsMonthlyTrend().reduce((max, point) => Math.max(max, point.value), 0),
  );

  constructor() {
    void this.loadDashboard();
  }

  trendWidth(value: number, max: number): number {
    if (max <= 0) {
      return 0;
    }

    return Math.max((value / max) * 100, value > 0 ? 8 : 0);
  }

  private async loadDashboard(): Promise<void> {
    this.loading.set(true);

    const totals = await this.loadTotals();
    this.totals.set(totals);

    const [bookings, screenings] = await Promise.all([
      this.loadBookingsSample(totals.bookings),
      this.loadScreeningsSample(totals.screenings),
    ]);

    this.computeMetrics(bookings, screenings);
    this.loading.set(false);
  }

  private async loadTotals(): Promise<DashboardTotals> {
    const [movies, users, halls, screenings, bookings] = await Promise.all([
      this.movieService.getMoviesPage(0, 1, { field: 'dateCreated', direction: 'DESC' }, {}, ''),
      this.userService.getUsersPage(0, 1, { field: 'email', direction: 'ASC' }, {}, ''),
      this.hallService.getHallsPage(0, 1, { field: 'dateCreated', direction: 'DESC' }),
      this.screeningService.getScreeningsPage(0, 1, { field: 'dateCreated', direction: 'DESC' }, {}),
      this.bookingService.getBookingsPage(0, 1, { field: 'dateCreated', direction: 'DESC' }, {}),
    ]);

    return {
      movies: movies.totalCount ?? 0,
      users: users.totalCount ?? 0,
      halls: halls.totalCount ?? 0,
      screenings: screenings.totalCount ?? 0,
      bookings: bookings.totalCount ?? 0,
    };
  }

  private async loadBookingsSample(total: number): Promise<Booking[]> {
    const take = Math.min(total, Dashboard.ANALYTICS_LIMIT);
    if (take === 0) {
      return [];
    }

    if (total > Dashboard.ANALYTICS_LIMIT) {
      this.analyticsNotice.set(
        `Analytics are based on the most recent ${Dashboard.ANALYTICS_LIMIT.toLocaleString()} bookings/screenings.`,
      );
    }

    const page = await this.bookingService.getBookingsPage(0, take, { field: 'dateCreated', direction: 'DESC' }, {});
    return page.items ?? [];
  }

  private async loadScreeningsSample(total: number): Promise<Screening[]> {
    const take = Math.min(total, Dashboard.ANALYTICS_LIMIT);
    if (take === 0) {
      return [];
    }

    const page = await this.screeningService.getScreeningsPage(0, take, { field: 'date', direction: 'DESC' }, {});
    return page.items ?? [];
  }

  private computeMetrics(bookings: Booking[], screenings: Screening[]): void {
    const now = new Date();
    const todayStart = this.startOfDay(now);
    const weekStart = this.startOfWeek(now);
    const weekEnd = this.addDays(weekStart, 7);
    const monthStart = new Date(now.getFullYear(), now.getMonth(), 1);
    const monthEnd = new Date(now.getFullYear(), now.getMonth() + 1, 1);

    const bookingDates = bookings
      .map((booking) => ({
        booking,
        date: this.parseDateTime(booking.dateCreated),
      }))
      .filter((entry) => !!entry.date) as Array<{ booking: Booking; date: Date }>;

    const screeningDates = screenings
      .map((screening) => ({
        screening,
        date: this.parseDateOnly(screening.date),
      }))
      .filter((entry) => !!entry.date) as Array<{ screening: Screening; date: Date }>;

    const bookingsThisWeek = bookingDates.filter((entry) => entry.date >= weekStart && entry.date < weekEnd);
    const bookingsThisMonth = bookingDates.filter((entry) => entry.date >= monthStart && entry.date < monthEnd);
    const screeningsThisWeek = screeningDates.filter((entry) => entry.date >= todayStart && entry.date < weekEnd);

    this.bookingsThisWeek.set(bookingsThisWeek.length);
    this.revenueThisWeek.set(bookingsThisWeek.reduce((sum, entry) => sum + (entry.booking.totalPrice ?? 0), 0));
    this.bookingsThisMonth.set(bookingsThisMonth.length);
    this.revenueThisMonth.set(bookingsThisMonth.reduce((sum, entry) => sum + (entry.booking.totalPrice ?? 0), 0));
    this.screeningsThisWeek.set(screeningsThisWeek.length);

    this.bookingsWeeklyTrend.set(this.buildWeeklyTrend(bookingDates.map((entry) => entry.date), 8));
    this.screeningsMonthlyTrend.set(this.buildMonthlyTrend(screeningDates.map((entry) => entry.date), 6));
  }

  private buildWeeklyTrend(dates: Date[], weeks: number): TrendPoint[] {
    const now = new Date();
    const currentWeekStart = this.startOfWeek(now);
    const points: TrendPoint[] = [];

    for (let index = weeks - 1; index >= 0; index -= 1) {
      const start = this.addDays(currentWeekStart, -index * 7);
      const end = this.addDays(start, 7);
      const value = dates.filter((date) => date >= start && date < end).length;
      points.push({
        label: `${start.toLocaleDateString('en-US', { month: 'short' })} ${start.getDate()}`,
        value,
      });
    }

    return points;
  }

  private buildMonthlyTrend(dates: Date[], months: number): TrendPoint[] {
    const now = new Date();
    const points: TrendPoint[] = [];

    for (let index = months - 1; index >= 0; index -= 1) {
      const start = new Date(now.getFullYear(), now.getMonth() - index, 1);
      const end = new Date(now.getFullYear(), now.getMonth() - index + 1, 1);
      const value = dates.filter((date) => date >= start && date < end).length;
      points.push({
        label: start.toLocaleDateString('en-US', { month: 'short' }),
        value,
      });
    }

    return points;
  }

  private startOfWeek(date: Date): Date {
    const day = date.getDay();
    const distanceFromMonday = day === 0 ? 6 : day - 1;
    return this.startOfDay(this.addDays(date, -distanceFromMonday));
  }

  private startOfDay(date: Date): Date {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
  }

  private addDays(date: Date, days: number): Date {
    const copy = new Date(date);
    copy.setDate(copy.getDate() + days);
    return copy;
  }

  private parseDateOnly(value: string | null | undefined): Date | null {
    if (!value) {
      return null;
    }

    const normalized = value.trim();
    if (!normalized) {
      return null;
    }

    const date = new Date(`${normalized}T00:00:00`);
    return Number.isNaN(date.getTime()) ? null : date;
  }

  private parseDateTime(value: string | null | undefined): Date | null {
    if (!value) {
      return null;
    }

    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
  }
}
