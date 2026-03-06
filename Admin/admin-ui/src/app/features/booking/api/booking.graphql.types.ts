import { SortDirection } from '../../../shared/types/sort-direction.type';

export interface Booking {
  id: string;
  userId: string;
  screeningId: string;
  seatIds: string[];
  totalPrice: number;
  dateCreated: string;
}

export type BookingSortField = 'dateCreated' | 'userId' | 'screeningId' | 'totalPrice';

export interface BookingSort {
  field: BookingSortField;
  direction: SortDirection;
}

export interface BookingFilters {
  userId?: string | string[];
  screeningId?: string | string[];
}

export interface BookingPage {
  items: Booking[];
  totalCount: number;
}

export interface BookingReferenceOption {
  id: string;
  label: string;
}
