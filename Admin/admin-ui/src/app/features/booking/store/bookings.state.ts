import { Booking, BookingFilters, BookingReferenceOption, BookingSort } from '../api/booking.graphql.types';

export interface BookingsState {
  bookings: Booking[];
  totalRecords: number;
  first: number;
  pageSize: number;
  sort: BookingSort;
  filters: BookingFilters;
  userFilterOptions: BookingReferenceOption[];
  screeningFilterOptions: BookingReferenceOption[];
  userLabels: Record<string, string>;
  screeningLabels: Record<string, string>;
}

export const DEFAULT_BOOKING_SORT: BookingSort = {
  field: 'dateCreated',
  direction: 'DESC',
};

export const initialBookingsState: BookingsState = {
  bookings: [],
  totalRecords: 0,
  first: 0,
  pageSize: 25,
  sort: DEFAULT_BOOKING_SORT,
  filters: {},
  userFilterOptions: [],
  screeningFilterOptions: [],
  userLabels: {},
  screeningLabels: {},
};
