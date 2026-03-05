import { User, UserFilters, UserSort } from '../api/user.graphql.types';

export interface UsersState {
  users: User[];
  totalRecords: number;
  first: number;
  pageSize: number;
  sort: UserSort;
  searchTerm: string;
  filters: UserFilters;
  roleFilterOptions: string[];
  statusFilterOptions: string[];
  providerFilterOptions: string[];
}

export const DEFAULT_USER_SORT: UserSort = {
  field: 'dateCreated',
  direction: 'DESC',
};

export const initialUsersState: UsersState = {
  users: [],
  totalRecords: 0,
  first: 0,
  pageSize: 25,
  sort: DEFAULT_USER_SORT,
  searchTerm: '',
  filters: {},
  roleFilterOptions: [],
  statusFilterOptions: [],
  providerFilterOptions: [],
};
