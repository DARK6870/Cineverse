import { SortDirection } from '../../../shared/types/sort-direction.type';

export interface Screening {
  id: string;
  movieId: string;
  hallId: string;
  date: string;
  startTime: string;
  endTime: string;
  ticketPrice: number;
  dateCreated: string;
}

export type ScreeningSortField = 'dateCreated' | 'movieId' | 'hallId' | 'date' | 'startTime' | 'endTime' | 'ticketPrice';

export interface ScreeningSort {
  field: ScreeningSortField;
  direction: SortDirection;
}

export interface ScreeningFilters {
  movieId?: string | string[];
  hallId?: string | string[];
  date?: string;
}

export interface ScreeningPage {
  items: Screening[];
  totalCount: number;
}

export interface CreateScreeningRequestInput {
  movieId: string;
  hallId: string;
  date: string;
  startTime: string;
  endTime: string;
  ticketPrice: number;
}

export interface UpdateScreeningRequestInput extends CreateScreeningRequestInput {
  id: string;
}

export interface ScreeningReferenceOption {
  id: string;
  label: string;
}
