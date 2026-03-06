import { Screening, ScreeningFilters, ScreeningReferenceOption, ScreeningSort } from '../api/screening.graphql.types';

export interface ScreeningsState {
  screenings: Screening[];
  totalRecords: number;
  first: number;
  pageSize: number;
  sort: ScreeningSort;
  filters: ScreeningFilters;
  movieFilterOptions: ScreeningReferenceOption[];
  hallFilterOptions: ScreeningReferenceOption[];
  movieLabels: Record<string, string>;
  hallLabels: Record<string, string>;
}

export const DEFAULT_SCREENING_SORT: ScreeningSort = {
  field: 'date',
  direction: 'DESC',
};

export const initialScreeningsState: ScreeningsState = {
  screenings: [],
  totalRecords: 0,
  first: 0,
  pageSize: 25,
  sort: DEFAULT_SCREENING_SORT,
  filters: {},
  movieFilterOptions: [],
  hallFilterOptions: [],
  movieLabels: {},
  hallLabels: {},
};
