import { Component, inject, OnInit, signal } from '@angular/core';
import { ScreeningGraphqlService } from '../../../screening/api/screening.graphql.service';
import { Screening } from '../../../screening/api/screening.graphql.types';
import { firstValueFrom } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../../core/services/toast.service';
import { JwtClaimsService } from '../../../../core/services/jwt-claims.service';
import { UserStatus } from '../../../../shared/models/jwt-payload.model';
import { Movie } from '../../../movie/api/movie.graphql.types';
import { MovieGraphqlService } from '../../../movie/api/movie.graphql.service';
import { MovieDetailsCard } from '../../../../shared/components/movie-details-card/movie-details-card';
import { formatDate } from '../../../../shared/utils/date-utils';
import { Hall } from '../../../hall/api/hall.graphql.types';
import { HallGraphqlService } from '../../../hall/api/hall.graphql.service';
import { SeatSelector } from '../../../../shared/components/seat-selector/seat-selector';
import { BookingGraphqlService } from '../../api/booking.graphql.service';
import { CreateBookingRequestInput } from '../../api/booking.graphql.types';

@Component({
  selector: 'app-create-booking',
  imports: [MovieDetailsCard, SeatSelector],
  standalone: true,
  templateUrl: 'create-booking.html',
  styleUrl: 'create-booking.css',
})
export class CreateBooking implements OnInit {
  private screeningGraphQlService = inject(ScreeningGraphqlService);
  private movieGraphQlService = inject(MovieGraphqlService);
  private hallGraphQlService = inject(HallGraphqlService);
  private bookingGraphQlService = inject(BookingGraphqlService);
  private route = inject(ActivatedRoute);
  private toastService = inject(ToastService);
  private jwtClaimsService = inject(JwtClaimsService);
  private router = inject(Router);

  screening = signal<Screening | null>(null);
  movie = signal<Movie | null>(null);
  hall = signal<Hall | null>(null);
  bookedSeats = signal<string[] | null>(null);

  async ngOnInit() {
    if (
      (await this.jwtClaimsService.decodeTokenAsync()).userStatus !=
      UserStatus.Normal
    ) {
      this.router.navigate(['/profile']).then(() => {
        this.toastService.warning('Please confirm your email');
      });
    }
    const screeningId = this.route.snapshot.paramMap.get('screeningId');
    if (!screeningId) {
      this.toastService.error('Invalid screening ID');
      return;
    }

    const screening = await firstValueFrom(
      this.screeningGraphQlService.getScreeningById(screeningId),
    );

    const movie = await firstValueFrom(
      this.movieGraphQlService.getMovieById(screening.movieId),
    );

    const hall = await firstValueFrom(
      this.hallGraphQlService.getHallById(screening.hallId),
    );

    const bookedSeats = await firstValueFrom(
      this.bookingGraphQlService.getBookedSeats(screeningId),
    );

    this.movie.set(movie);
    this.screening.set(screening);
    this.hall.set(hall);
    this.bookedSeats.set(bookedSeats);
  }

  protected readonly formatDate = formatDate;

  protected async handleConfirm($event: string[]) {
    const request: CreateBookingRequestInput = {
      screeningId: this.screening()!.id,
      seatsIds: $event,
    };

    const result = await firstValueFrom(
      this.bookingGraphQlService.createBooking(request)
    );

    if (result) {
      this.router.navigate(['/']).then(() =>
        this.toastService.success('Booking successfully created')
      );
    }
  }
}
