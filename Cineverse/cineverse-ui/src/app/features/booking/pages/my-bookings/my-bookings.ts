import { Component, inject, OnInit, signal } from '@angular/core';
import { Booking } from '../../api/booking.graphql.types';
import { Movie } from '../../../movie/api/movie.graphql.types';
import { BookingGraphqlService } from '../../api/booking.graphql.service';
import { MovieGraphqlService } from '../../../movie/api/movie.graphql.service';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Screening } from '../../../screening/api/screening.graphql.types';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-bookings',
  imports: [DatePipe, RouterLink],
  standalone: true,
  templateUrl: 'my-bookings.html',
  styleUrl: 'my-bookings.css',
})
export class MyBookings implements OnInit {
  private bookingApiService = inject(BookingGraphqlService);
  private screeningApiService = inject(ScreeningGraphqlService);
  private movieApiService = inject(MovieGraphqlService);

  public bookings = signal<Booking[]>([]);
  public movies = signal<Movie[]>([]);
  public screenings = signal<Screening[]>([]);

  async ngOnInit() {
    const bookings = await this.bookingApiService.getUserBookings();
    this.bookings.set(bookings);

    const screeningIds = bookings.map((b) => b.screeningId);
    const screenings =
      await this.screeningApiService.getScreeningsByIds(screeningIds);
    this.screenings.set(screenings);

    const movieIds = screenings.map((s) => s.movieId);
    const movies = await this.movieApiService.getMoviesByIds(movieIds);
    this.movies.set(movies);
  }

  getScreening(screeningId: string): Screening | undefined {
    return this.screenings().find((s) => s.id === screeningId);
  }

  getMovie(screeningId: string): Movie | undefined {
    const screening = this.getScreening(screeningId);
    return this.movies().find((m) => m.id === screening?.movieId);
  }

  getStatus(
    screening: Screening | undefined,
  ): 'Upcoming' | 'Watched' | 'Cancelled' {
    if (!screening) return 'Watched';
    const now = new Date();
    const screeningDate = new Date(`${screening.date}T${screening.startTime}`);
    return screeningDate > now ? 'Upcoming' : 'Watched';
  }
}
