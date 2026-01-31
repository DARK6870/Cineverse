import { Component, inject, OnInit, signal } from '@angular/core';
import { MovieDetailsCard } from "../../../../shared/components/movie-details-card/movie-details-card";
import { SeatSelector } from "../../../../shared/components/seat-selector/seat-selector";
import { MovieGraphqlService } from '../../../movie/api/movie.graphql.service';
import { ActivatedRoute } from '@angular/router';
import { BookingGraphqlService } from '../../api/booking.graphql.service';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Screening } from '../../../screening/api/screening.graphql.types';
import { Movie } from '../../../movie/api/movie.graphql.types';
import { Hall } from '../../../hall/api/hall.graphql.types';
import { HallGraphqlService } from '../../../hall/api/hall.graphql.service';
import { Booking as BookingModel } from '../../api/booking.graphql.types';

@Component({
  selector: 'app-booking',
  imports: [MovieDetailsCard, SeatSelector],
  standalone: true,
  templateUrl: 'booking.html',
  styleUrl: 'booking.css',
})
export class Booking implements OnInit {
  private movieApiService = inject(MovieGraphqlService);
  private bookingApiService = inject(BookingGraphqlService);
  private screeningApiService = inject(ScreeningGraphqlService);
  private hallApiService = inject(HallGraphqlService);
  private activatedRoute = inject(ActivatedRoute);

  booking = signal<BookingModel | null>(null);
  screening = signal<Screening | null>(null);
  movie = signal<Movie | null>(null);
  hall = signal<Hall | null>(null);

  async ngOnInit() {
    const bookingId : string = this.activatedRoute.snapshot.params['bookingId'];

    const booking = await this.bookingApiService.getBookingById(bookingId);
    const screening = await this.screeningApiService.getScreeningById(booking.screeningId);
    const hall = await this.hallApiService.getHallById(screening.hallId);
    const movie = await this.movieApiService.getMovieById(screening.movieId);

    this.booking.set(booking);
    this.screening.set(screening);
    this.movie.set(movie);
    this.hall.set(hall);
  }
}
