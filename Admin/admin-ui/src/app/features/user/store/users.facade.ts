import { inject, Injectable } from '@angular/core';
import { User, UserFilters, UserSort, UpdateUserRequestInput } from '../api/user.graphql.types';
import { UserGraphqlService } from '../api/user.graphql.service';
import { UsersStore } from './users.store';

@Injectable({ providedIn: 'root' })
export class UsersFacade {
  private store = inject(UsersStore);
  private userService = inject(UserGraphqlService);

  readonly users = this.store.users;
  readonly totalRecords = this.store.totalRecords;
  readonly pageSize = this.store.pageSize;
  readonly first = this.store.first;
  readonly sort = this.store.sort;
  readonly sortField = this.store.sortField;
  readonly sortOrder = this.store.sortOrder;
  readonly searchTerm = this.store.searchTerm;
  readonly filters = this.store.filters;
  readonly roleFilter = this.store.roleFilter;
  readonly statusFilter = this.store.statusFilter;
  readonly providerFilter = this.store.providerFilter;
  readonly roleFilterOptions = this.store.roleFilterOptions;
  readonly statusFilterOptions = this.store.statusFilterOptions;
  readonly providerFilterOptions = this.store.providerFilterOptions;

  loadPage(first: number, pageSize: number, sort?: UserSort, filters?: UserFilters, searchTerm?: string): void {
    void this.loadPageInternal(first, pageSize, sort, filters, searchTerm);
  }

  applySearch(searchTerm: string): void {
    void this.loadPageInternal(0, this.pageSize(), this.sort(), this.filters(), searchTerm.trim());
  }

  applyFilters(filters: UserFilters): void {
    void this.loadPageInternal(0, this.pageSize(), this.sort(), filters, this.searchTerm());
  }

  loadRoleFilterOptions(): void {
    if (this.roleFilterOptions().length > 0) {
      return;
    }

    void this.loadRoleFilterOptionsInternal();
  }

  loadStatusFilterOptions(): void {
    if (this.statusFilterOptions().length > 0) {
      return;
    }

    void this.loadStatusFilterOptionsInternal();
  }

  loadProviderFilterOptions(): void {
    if (this.providerFilterOptions().length > 0) {
      return;
    }

    void this.loadProviderFilterOptionsInternal();
  }

  refresh(): void {
    void this.loadPageInternal(this.first(), this.pageSize(), this.sort(), this.filters(), this.searchTerm());
  }

  getUserById(id: string): Promise<User> {
    return this.userService.getUserById(id);
  }

  async updateUser(request: UpdateUserRequestInput): Promise<void> {
    await this.userService.updateUser(request);
    this.refresh();
  }

  private async loadPageInternal(
    first: number,
    pageSize: number,
    sort?: UserSort,
    filters?: UserFilters,
    searchTerm?: string,
  ): Promise<void> {
    const effectiveSort = sort ?? this.sort();
    const effectiveFilters = filters ?? this.filters();
    const effectiveSearchTerm = searchTerm ?? this.searchTerm();

    this.store.setQuery(first, pageSize, effectiveSort, effectiveFilters, effectiveSearchTerm);
    const data = await this.userService.getUsersPage(first, pageSize, effectiveSort, effectiveFilters, effectiveSearchTerm);
    this.store.setPageData(data.items ?? [], data.totalCount ?? 0);
  }

  private async loadRoleFilterOptionsInternal(): Promise<void> {
    const roles = await this.userService.getUsersDistinctFilterValues('Role');
    this.store.setRoleFilterOptions(roles);
  }

  private async loadStatusFilterOptionsInternal(): Promise<void> {
    const statuses = await this.userService.getUsersDistinctFilterValues('Status');
    this.store.setStatusFilterOptions(statuses);
  }

  private async loadProviderFilterOptionsInternal(): Promise<void> {
    const providers = await this.userService.getUsersDistinctFilterValues('Provider');
    this.store.setProviderFilterOptions(providers);
  }
}
