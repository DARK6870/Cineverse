import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit, inject } from '@angular/core';
import { ButtonDirective } from 'primeng/button';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { BookingFilters, BookingSort } from '../../api/booking.graphql.types';
import { BookingsFacade } from '../../store/bookings.facade';

@Component({
  selector: 'app-bookings',
  imports: [CommonModule, TableModule, ButtonDirective],
  templateUrl: './bookings.html',
  styleUrl: './bookings.css',
  standalone: true,
})
export class Bookings implements OnInit {
  protected facade = inject(BookingsFacade);

  protected isUserFilterOpen = false;
  protected isScreeningFilterOpen = false;

  protected selectedUserIds: string[] = [];
  protected selectedScreeningIds: string[] = [];

  protected userSearchTerm = '';
  protected screeningSearchTerm = '';

  tableHeight = 'calc(100vh - 300px)';

  private readonly sortableColumns: Record<string, BookingSort['field']> = {
    totalPrice: 'totalPrice',
    dateCreated: 'dateCreated',
  };

  ngOnInit(): void {
    this.selectedUserIds = this.currentUserSelection();
    this.selectedScreeningIds = this.currentScreeningSelection();
    this.facade.ensureReferenceOptionsLoaded();
  }

  onLazyLoad(event: TableLazyLoadEvent): void {
    const first = event.first ?? this.facade.first();
    const pageSize = event.rows ?? this.facade.pageSize();
    const sort = this.getSortFromEvent(event.sortField, event.sortOrder);

    this.facade.loadPage(first, pageSize, sort);
  }

  protected toggleUserFilterMenu(): void {
    const willOpen = !this.isUserFilterOpen;
    this.isUserFilterOpen = willOpen;
    if (willOpen) {
      this.facade.ensureReferenceOptionsLoaded();
      this.selectedUserIds = this.currentUserSelection();
      this.isScreeningFilterOpen = false;
    }
  }

  protected toggleScreeningFilterMenu(): void {
    const willOpen = !this.isScreeningFilterOpen;
    this.isScreeningFilterOpen = willOpen;
    if (willOpen) {
      this.facade.ensureReferenceOptionsLoaded();
      this.selectedScreeningIds = this.currentScreeningSelection();
      this.isUserFilterOpen = false;
    }
  }

  protected toggleUserSelection(userId: string, checked: boolean): void {
    this.selectedUserIds = this.toggleSelection(this.selectedUserIds, userId, checked);
  }

  protected toggleScreeningSelection(screeningId: string, checked: boolean): void {
    this.selectedScreeningIds = this.toggleSelection(this.selectedScreeningIds, screeningId, checked);
  }

  protected applyUserFilters(): void {
    this.applySelectionFilters(this.selectedUserIds, this.currentScreeningSelection());
    this.isUserFilterOpen = false;
  }

  protected applyScreeningFilters(): void {
    this.applySelectionFilters(this.currentUserSelection(), this.selectedScreeningIds);
    this.isScreeningFilterOpen = false;
  }

  protected clearUserFilters(): void {
    this.selectedUserIds = [];
  }

  protected clearScreeningFilters(): void {
    this.selectedScreeningIds = [];
  }

  protected isUserSelected(userId: string): boolean {
    return this.selectedUserIds.includes(userId);
  }

  protected isScreeningSelected(screeningId: string): boolean {
    return this.selectedScreeningIds.includes(screeningId);
  }

  protected userFilterButtonLabel(): string {
    return this.getFilterButtonLabel('User', this.facade.userFilter().length);
  }

  protected screeningFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Screening', this.facade.screeningFilter().length);
  }

  protected filteredUserOptions() {
    const normalizedSearch = this.userSearchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return this.facade.userFilterOptions();
    }

    return this.facade.userFilterOptions().filter((option) => option.label.toLowerCase().includes(normalizedSearch));
  }

  protected filteredScreeningOptions() {
    const normalizedSearch = this.screeningSearchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return this.facade.screeningFilterOptions();
    }

    return this.facade.screeningFilterOptions().filter((option) => option.label.toLowerCase().includes(normalizedSearch));
  }

  protected userLabel(userId: string): string {
    return this.facade.getUserLabel(userId);
  }

  protected screeningLabel(screeningId: string): string {
    return this.facade.getScreeningLabel(screeningId);
  }

  protected seatSummary(seatIds: string[] | null | undefined): string {
    const seats = seatIds ?? [];
    if (seats.length === 0) {
      return '—';
    }

    if (seats.length <= 3) {
      return seats.join(', ');
    }

    return `${seats.slice(0, 3).join(', ')} +${seats.length - 3}`;
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
    this.isUserFilterOpen = false;
    this.isScreeningFilterOpen = false;
  }

  private getFilterButtonLabel(baseLabel: string, count: number): string {
    if (count === 0) {
      return baseLabel;
    }

    return `${baseLabel} (${count})`;
  }

  private getSortFromEvent(
    sortField: string | string[] | null | undefined,
    sortOrder: number | null | undefined,
  ): BookingSort | undefined {
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

  private applySelectionFilters(selectedUserIds: string[], selectedScreeningIds: string[]): void {
    const filters: BookingFilters = {
      userId: selectedUserIds.length > 0 ? selectedUserIds : undefined,
      screeningId: selectedScreeningIds.length > 0 ? selectedScreeningIds : undefined,
    };

    this.facade.applyFilters(filters);
  }

  private currentUserSelection(): string[] {
    return [...this.facade.userFilter()];
  }

  private currentScreeningSelection(): string[] {
    return [...this.facade.screeningFilter()];
  }

  private toggleSelection(values: string[], value: string, checked: boolean): string[] {
    if (checked) {
      return values.includes(value) ? values : [...values, value];
    }

    return values.filter((item) => item !== value);
  }
}
