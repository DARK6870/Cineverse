import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import {
  createBookingMutation,
  getBookedSeatsQuery,
  getBookingByIdQuery,
} from './booking.graphql';
import { Booking, CreateBookingRequestInput } from './booking.graphql.types';

@Injectable({ providedIn: 'root' })
export class BookingGraphqlService {
  private apollo = inject(Apollo);

  public getBookingById(id: string): Promise<Booking> {
    return firstValueFrom(
      this.apollo
        .query<{ bookingById: Booking }>({
          ...getBookingByIdQuery(id),
        })
        .pipe(map((res) => res.data.bookingById)),
    );
  }

  public getBookedSeats(screeningId: string): Promise<string[]> {
    return firstValueFrom(
      this.apollo
        .query<{ bookedSeats: string[] }>({
          ...getBookedSeatsQuery(screeningId),
        })
        .pipe(map((res) => res.data.bookedSeats)),
    );
  }

  public createBooking(request: CreateBookingRequestInput): Promise<boolean> {
    return firstValueFrom(
      this.apollo
        .mutate<{ createBooking: boolean }>({
          ...createBookingMutation(request),
        })
        .pipe(map((res) => res.data!.createBooking)),
    );
  }
}
