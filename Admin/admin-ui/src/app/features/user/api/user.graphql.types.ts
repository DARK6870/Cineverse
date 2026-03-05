import { SortDirection } from '../../../shared/types/sort-direction.type';

export type UserRole = 'Admin' | 'Manager' | 'User';
export type UserStatus = 'PendingEmailConfirmation' | 'Normal' | 'Blocked' | 'Disabled';

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  provider: string;
  role: UserRole;
  status: UserStatus;
}

export interface UserList {
  items: User[];
  totalCount: number;
}

export interface UserFilters {
  status?: UserStatus[];
  provider?: string[];
  role?: UserRole[];
}

export interface UserPage {
  items: User[];
  totalCount: number;
}

export interface UpdateUserRequestInput {
  id: string;
  role?: UserRole;
  status?: UserStatus;
}

export interface UserSort {
  field: 'dateCreated' | 'email' | 'firstName' | 'lastName' | 'provider' | 'role' | 'status';
  direction: SortDirection;
}

export type UserDistinctField = 'Status' | 'Provider' | 'Role';
