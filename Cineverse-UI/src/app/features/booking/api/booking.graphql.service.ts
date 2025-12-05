import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { createBookingMutation, getBookedSeatsQuery } from './booking.graphql';
import { CreateBookingRequestInput } from './booking.graphql.types';

@Injectable({providedIn: 'root'})
export class BookingGraphqlService {
  private apollo = inject(Apollo);

  public getBookedSeats(screeningId: string): Observable<string[]> {
    return this.apollo
      .query<{bookedSeats: string[]}>({
        ...getBookedSeatsQuery(screeningId)
      })
      .pipe(map((res) => res.data.bookedSeats));
  }

  public createBooking(request: CreateBookingRequestInput): Observable<boolean> {
    return this.apollo.mutate<{createBooking: boolean}>({
      ...createBookingMutation(request),
    }).pipe(map(res => res.data!.createBooking));
  }
}
