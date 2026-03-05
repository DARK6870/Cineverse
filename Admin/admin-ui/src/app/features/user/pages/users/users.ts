import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonDirective } from 'primeng/button';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { highlightSearchMatch } from '../../../../shared/utils/search-highlight.util';
import { UserFilters, UserRole, UserSort, UserStatus } from '../../api/user.graphql.types';
import { UsersFacade } from '../../store/users.facade';

@Component({
  selector: 'app-users',
  imports: [CommonModule, TableModule, ButtonDirective, RouterLink],
  templateUrl: 'users.html',
  styleUrl: 'users.css',
  standalone: true,
})
export class Users implements OnInit {
  protected facade = inject(UsersFacade);

  protected isRoleFilterOpen = false;
  protected isStatusFilterOpen = false;
  protected isProviderFilterOpen = false;

  protected selectedRoles: string[] = [];
  protected selectedStatuses: string[] = [];
  protected selectedProviders: string[] = [];

  protected roleSearchTerm = '';
  protected statusSearchTerm = '';
  protected providerSearchTerm = '';

  tableHeight = 'calc(100vh - 300px)';
  private readonly sortableColumns: Record<string, UserSort['field']> = {
    email: 'email',
    firstName: 'firstName',
    lastName: 'lastName',
    provider: 'provider',
    role: 'role',
    status: 'status',
  };

  ngOnInit(): void {
    this.selectedRoles = this.currentRoleSelection();
    this.selectedStatuses = this.currentStatusSelection();
    this.selectedProviders = this.currentProviderSelection();
  }

  onLazyLoad(event: TableLazyLoadEvent): void {
    const first = event.first ?? this.facade.first();
    const pageSize = event.rows ?? this.facade.pageSize();
    const sort = this.getSortFromEvent(event.sortField, event.sortOrder);
    this.facade.loadPage(first, pageSize, sort);
  }

  protected applySearch(searchTerm: string): void {
    this.facade.applySearch(searchTerm);
  }

  protected toggleRoleFilterMenu(): void {
    const willOpen = !this.isRoleFilterOpen;
    this.isRoleFilterOpen = willOpen;
    if (willOpen) {
      this.facade.loadRoleFilterOptions();
      this.selectedRoles = this.currentRoleSelection();
      this.closeFilterMenus('role');
    }
  }

  protected toggleStatusFilterMenu(): void {
    const willOpen = !this.isStatusFilterOpen;
    this.isStatusFilterOpen = willOpen;
    if (willOpen) {
      this.facade.loadStatusFilterOptions();
      this.selectedStatuses = this.currentStatusSelection();
      this.closeFilterMenus('status');
    }
  }

  protected toggleProviderFilterMenu(): void {
    const willOpen = !this.isProviderFilterOpen;
    this.isProviderFilterOpen = willOpen;
    if (willOpen) {
      this.facade.loadProviderFilterOptions();
      this.selectedProviders = this.currentProviderSelection();
      this.closeFilterMenus('provider');
    }
  }

  protected toggleRoleSelection(value: string, checked: boolean): void {
    this.selectedRoles = this.toggleSelection(this.selectedRoles, value, checked);
  }

  protected toggleStatusSelection(value: string, checked: boolean): void {
    this.selectedStatuses = this.toggleSelection(this.selectedStatuses, value, checked);
  }

  protected toggleProviderSelection(value: string, checked: boolean): void {
    this.selectedProviders = this.toggleSelection(this.selectedProviders, value, checked);
  }

  protected applyRoleFilters(): void {
    this.applySelectionFilters(this.selectedRoles, this.currentStatusSelection(), this.currentProviderSelection());
    this.isRoleFilterOpen = false;
  }

  protected applyStatusFilters(): void {
    this.applySelectionFilters(this.currentRoleSelection(), this.selectedStatuses, this.currentProviderSelection());
    this.isStatusFilterOpen = false;
  }

  protected applyProviderFilters(): void {
    this.applySelectionFilters(this.currentRoleSelection(), this.currentStatusSelection(), this.selectedProviders);
    this.isProviderFilterOpen = false;
  }

  protected clearRoleFilters(): void {
    this.selectedRoles = [];
  }

  protected clearStatusFilters(): void {
    this.selectedStatuses = [];
  }

  protected clearProviderFilters(): void {
    this.selectedProviders = [];
  }

  protected isRoleSelected(value: string): boolean {
    return this.selectedRoles.includes(value);
  }

  protected isStatusSelected(value: string): boolean {
    return this.selectedStatuses.includes(value);
  }

  protected isProviderSelected(value: string): boolean {
    return this.selectedProviders.includes(value);
  }

  protected roleFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Role', this.facade.roleFilter().length);
  }

  protected statusFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Status', this.facade.statusFilter().length);
  }

  protected providerFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Provider', this.facade.providerFilter().length);
  }

  protected filteredRoleOptions(): string[] {
    return this.filterOptions(this.facade.roleFilterOptions(), this.roleSearchTerm);
  }

  protected filteredStatusOptions(): string[] {
    return this.filterOptions(this.facade.statusFilterOptions(), this.statusSearchTerm);
  }

  protected filteredProviderOptions(): string[] {
    return this.filterOptions(this.facade.providerFilterOptions(), this.providerSearchTerm);
  }

  protected fullName(firstName: string, lastName: string): string {
    return `${firstName ?? ''} ${lastName ?? ''}`.trim();
  }

  protected highlightSearchTerm(value: string | null | undefined): string {
    return highlightSearchMatch(value, this.facade.searchTerm());
  }

  @HostListener('document:click', ['$event'])
  protected onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement | null;
    if (target?.closest('.filter-menu')) {
      return;
    }

    this.closeAllFilterMenus();
  }

  private closeAllFilterMenus(): void {
    this.isRoleFilterOpen = false;
    this.isStatusFilterOpen = false;
    this.isProviderFilterOpen = false;
  }

  private closeFilterMenus(activeMenu: 'role' | 'status' | 'provider'): void {
    if (activeMenu !== 'role') {
      this.isRoleFilterOpen = false;
    }

    if (activeMenu !== 'status') {
      this.isStatusFilterOpen = false;
    }

    if (activeMenu !== 'provider') {
      this.isProviderFilterOpen = false;
    }
  }

  private currentRoleSelection(): string[] {
    return [...this.facade.roleFilter()];
  }

  private currentStatusSelection(): string[] {
    return [...this.facade.statusFilter()];
  }

  private currentProviderSelection(): string[] {
    return [...this.facade.providerFilter()];
  }

  private applySelectionFilters(roles: string[], statuses: string[], providers: string[]): void {
    const filters: UserFilters = {
      role: roles.length ? (roles as UserRole[]) : undefined,
      status: statuses.length ? (statuses as UserStatus[]) : undefined,
      provider: providers.length ? providers : undefined,
    };
    this.facade.applyFilters(filters);
  }

  private getFilterButtonLabel(baseLabel: string, count: number): string {
    if (count === 0) {
      return baseLabel;
    }

    return `${baseLabel} (${count})`;
  }

  private filterOptions<T extends string>(values: T[], searchTerm: string): T[] {
    const normalizedSearch = searchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return values;
    }

    return values.filter((value) => value.toLowerCase().includes(normalizedSearch));
  }

  private toggleSelection<T extends string>(values: T[], value: T, checked: boolean): T[] {
    if (checked) {
      return values.includes(value) ? values : [...values, value];
    }

    return values.filter((item) => item !== value);
  }

  private getSortFromEvent(
    sortField: string | string[] | null | undefined,
    sortOrder: number | null | undefined,
  ): UserSort | undefined {
    if (sortOrder === 0) {
      return {
        field: 'dateCreated',
        direction: 'DESC',
      };
    }

    if (typeof sortField !== 'string' || (sortOrder !== 1 && sortOrder !== -1)) {
      return undefined;
    }

    const field = this.sortableColumns[sortField];
    if (!field) {
      return undefined;
    }

    return {
      field,
      direction: sortOrder === 1 ? 'ASC' : 'DESC',
    };
  }
}
