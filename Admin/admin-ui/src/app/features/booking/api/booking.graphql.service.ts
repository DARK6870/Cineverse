import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import { APOLLO_CLIENTS } from '../../../apollo/apollo.config';
import { BookingFilters, BookingPage, BookingSort } from './booking.graphql.types';
import { getBookingsPageQuery } from './booking.graphql';

@Injectable({ providedIn: 'root' })
export class BookingGraphqlService {
  private apollo = inject(Apollo);
  private cineverseClient = this.apollo.use(APOLLO_CLIENTS.CINEVERSE);

  public getBookingsPage(
    skip: number,
    take: number,
    sort: BookingSort,
    filters: BookingFilters,
  ): Promise<BookingPage> {
    return firstValueFrom(
      this.cineverseClient
        .query<{ bookings: BookingPage }>({
          ...getBookingsPageQuery(skip, take, sort, filters),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.bookings)),
    );
  }
}
