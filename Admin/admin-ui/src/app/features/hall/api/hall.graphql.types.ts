import { SortDirection } from '../../../shared/types/sort-direction.type';

export interface Hall {
  id: string;
  name: string;
  seats: Seat[];
  dateCreated: string;
}

export interface Seat {
  seatId: string;
  row: number;
  number: number;
}

export interface HallSeatInput {
  row: number;
  number: number;
}

export interface CreateHallRequestInput {
  name: string;
  seats: HallSeatInput[];
}

export interface UpdateHallRequestInput extends CreateHallRequestInput {
  id: string;
}

export type HallSortField = 'dateCreated' | 'name';

export interface HallSort {
  field: HallSortField;
  direction: SortDirection;
}

export interface HallPage {
  items: Hall[];
  totalCount: number;
}
