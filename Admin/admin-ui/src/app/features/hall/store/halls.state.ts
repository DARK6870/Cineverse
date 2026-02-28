import { Hall, HallSort } from '../api/hall.graphql.types';

export interface HallsState {
  halls: Hall[];
  totalRecords: number;
  first: number;
  pageSize: number;
  sort: HallSort;
}

export const DEFAULT_SORT: HallSort = {
  field: 'dateCreated',
  direction: 'DESC',
};

export const initialHallsState: HallsState = {
  halls: [],
  totalRecords: 0,
  first: 0,
  pageSize: 25,
  sort: DEFAULT_SORT,
};
