import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { createBookingMutation, getBookedSeatsQuery, getBookingByIdQuery } from './booking.graphql';
import { Booking, CreateBookingRequestInput } from './booking.graphql.types';

@Injectable({providedIn: 'root'})
export class BookingGraphqlService {
  private apollo = inject(Apollo);

  public getBookingById(id: string): Observable<Booking> {
    return this.apollo
      .query<{ bookingById: Booking }>({
        ...getBookingByIdQuery(id)
      })
      .pipe(map(res => res.data.bookingById));
  }

  public getBookedSeats(screeningId: string): Observable<string[]> {
    return this.apollo
      .query<{ bookedSeats: string[] }>({
        ...getBookedSeatsQuery(screeningId)
      })
      .pipe(map((res) => res.data.bookedSeats));
  }

  public createBooking(request: CreateBookingRequestInput): Observable<boolean> {
    return this.apollo.mutate<{ createBooking: boolean }>({
      ...createBookingMutation(request),
    }).pipe(map(res => res.data!.createBooking));
  }
}
