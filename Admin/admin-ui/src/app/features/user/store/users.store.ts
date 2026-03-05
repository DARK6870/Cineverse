import { Injectable, computed, signal } from '@angular/core';
import { User, UserFilters, UserSort } from '../api/user.graphql.types';
import { UsersState, initialUsersState } from './users.state';

@Injectable({ providedIn: 'root' })
export class UsersStore {
  private state = signal<UsersState>(initialUsersState);

  readonly users = computed(() => this.state().users);
  readonly totalRecords = computed(() => this.state().totalRecords);
  readonly first = computed(() => this.state().first);
  readonly pageSize = computed(() => this.state().pageSize);
  readonly sort = computed(() => this.state().sort);
  readonly sortField = computed(() => this.state().sort.field);
  readonly sortOrder = computed(() => (this.state().sort.direction === 'ASC' ? 1 : -1));
  readonly searchTerm = computed(() => this.state().searchTerm);
  readonly filters = computed(() => this.state().filters);
  readonly roleFilter = computed(() => this.state().filters.role ?? []);
  readonly statusFilter = computed(() => this.state().filters.status ?? []);
  readonly providerFilter = computed(() => this.state().filters.provider ?? []);
  readonly roleFilterOptions = computed(() => this.getDistinctFilterOptions(this.state().roleFilterOptions));
  readonly statusFilterOptions = computed(() => this.getDistinctFilterOptions(this.state().statusFilterOptions));
  readonly providerFilterOptions = computed(() => this.getDistinctFilterOptions(this.state().providerFilterOptions));

  setQuery(first: number, pageSize: number, sort: UserSort, filters: UserFilters, searchTerm: string): void {
    this.state.update((s) => ({
      ...s,
      first,
      pageSize,
      sort,
      filters,
      searchTerm,
    }));
  }

  setPageData(users: User[], totalRecords: number): void {
    this.state.update((s) => ({
      ...s,
      users,
      totalRecords,
    }));
  }

  setRoleFilterOptions(values: string[]): void {
    this.state.update((s) => ({
      ...s,
      roleFilterOptions: values,
    }));
  }

  setStatusFilterOptions(values: string[]): void {
    this.state.update((s) => ({
      ...s,
      statusFilterOptions: values,
    }));
  }

  setProviderFilterOptions(values: string[]): void {
    this.state.update((s) => ({
      ...s,
      providerFilterOptions: values,
    }));
  }

  private getDistinctFilterOptions(values: string[]): string[] {
    return [...new Set(values.filter((value) => !!value?.trim()))].sort((a, b) => a.localeCompare(b));
  }
}
