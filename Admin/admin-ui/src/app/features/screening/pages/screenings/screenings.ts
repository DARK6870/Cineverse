import { CommonModule } from '@angular/common';
import { Component, HostListener, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { ButtonDirective } from 'primeng/button';
import { ScreeningsFacade } from '../../store/screenings.facade';
import { ScreeningFilters, ScreeningSort } from '../../api/screening.graphql.types';

@Component({
  selector: 'app-screenings',
  imports: [CommonModule, TableModule, ButtonDirective, RouterLink],
  templateUrl: './screenings.html',
  styleUrl: './screenings.css',
  standalone: true,
})
export class Screenings implements OnInit {
  protected facade = inject(ScreeningsFacade);

  protected isMovieFilterOpen = false;
  protected isHallFilterOpen = false;
  protected isDateFilterOpen = false;

  protected selectedMovieIds: string[] = [];
  protected selectedHallIds: string[] = [];
  protected selectedDate = '';

  protected movieSearchTerm = '';
  protected hallSearchTerm = '';

  tableHeight = 'calc(100vh - 300px)';

  private readonly sortableColumns: Record<string, ScreeningSort['field']> = {
    date: 'date',
    startTime: 'startTime',
    endTime: 'endTime',
    ticketPrice: 'ticketPrice',
  };

  ngOnInit(): void {
    this.selectedMovieIds = this.currentMovieSelection();
    this.selectedHallIds = this.currentHallSelection();
    this.selectedDate = this.facade.dateFilter();
    this.facade.ensureReferenceOptionsLoaded();
  }

  onLazyLoad(event: TableLazyLoadEvent): void {
    const first = event.first ?? this.facade.first();
    const pageSize = event.rows ?? this.facade.pageSize();
    const sort = this.getSortFromEvent(event.sortField, event.sortOrder);

    this.facade.loadPage(first, pageSize, sort);
  }

  protected toggleMovieFilterMenu(): void {
    const willOpen = !this.isMovieFilterOpen;
    this.isMovieFilterOpen = willOpen;
    if (this.isMovieFilterOpen) {
      this.facade.ensureReferenceOptionsLoaded();
      this.selectedMovieIds = this.currentMovieSelection();
      this.isHallFilterOpen = false;
      this.isDateFilterOpen = false;
    }
  }

  protected toggleHallFilterMenu(): void {
    const willOpen = !this.isHallFilterOpen;
    this.isHallFilterOpen = willOpen;
    if (this.isHallFilterOpen) {
      this.facade.ensureReferenceOptionsLoaded();
      this.selectedHallIds = this.currentHallSelection();
      this.isMovieFilterOpen = false;
      this.isDateFilterOpen = false;
    }
  }

  protected toggleDateFilterMenu(): void {
    const willOpen = !this.isDateFilterOpen;
    this.isDateFilterOpen = willOpen;
    if (this.isDateFilterOpen) {
      this.selectedDate = this.facade.dateFilter();
      this.isMovieFilterOpen = false;
      this.isHallFilterOpen = false;
    }
  }

  protected toggleMovieSelection(movieId: string, checked: boolean): void {
    this.selectedMovieIds = this.toggleSelection(this.selectedMovieIds, movieId, checked);
  }

  protected toggleHallSelection(hallId: string, checked: boolean): void {
    this.selectedHallIds = this.toggleSelection(this.selectedHallIds, hallId, checked);
  }

  protected applyMovieFilters(): void {
    this.applySelectionFilters(this.selectedMovieIds, this.currentHallSelection(), this.facade.dateFilter());
    this.isMovieFilterOpen = false;
  }

  protected applyHallFilters(): void {
    this.applySelectionFilters(this.currentMovieSelection(), this.selectedHallIds, this.facade.dateFilter());
    this.isHallFilterOpen = false;
  }

  protected applyDateFilter(): void {
    this.applySelectionFilters(this.currentMovieSelection(), this.currentHallSelection(), this.selectedDate);
    this.isDateFilterOpen = false;
  }

  protected clearMovieFilters(): void {
    this.selectedMovieIds = [];
  }

  protected clearHallFilters(): void {
    this.selectedHallIds = [];
  }

  protected clearDateFilter(): void {
    this.selectedDate = '';
  }

  protected isMovieSelected(movieId: string): boolean {
    return this.selectedMovieIds.includes(movieId);
  }

  protected isHallSelected(hallId: string): boolean {
    return this.selectedHallIds.includes(hallId);
  }

  protected movieFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Movie', this.facade.movieFilter().length);
  }

  protected hallFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Hall', this.facade.hallFilter().length);
  }

  protected dateFilterButtonLabel(): string {
    const currentDateFilter = this.facade.dateFilter();
    if (!currentDateFilter) {
      return 'Date';
    }

    return `Date (${currentDateFilter})`;
  }

  protected filteredMovieOptions() {
    const normalizedSearch = this.movieSearchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return this.facade.movieFilterOptions();
    }

    return this.facade.movieFilterOptions().filter((option) => option.label.toLowerCase().includes(normalizedSearch));
  }

  protected filteredHallOptions() {
    const normalizedSearch = this.hallSearchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return this.facade.hallFilterOptions();
    }

    return this.facade.hallFilterOptions().filter((option) => option.label.toLowerCase().includes(normalizedSearch));
  }

  protected movieLabel(movieId: string): string {
    return this.facade.getMovieLabel(movieId);
  }

  protected hallLabel(hallId: string): string {
    return this.facade.getHallLabel(hallId);
  }

  protected formatTime(time: string): string {
    return time?.slice(0, 5) || '—';
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
    this.isMovieFilterOpen = false;
    this.isHallFilterOpen = false;
    this.isDateFilterOpen = false;
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
  ): ScreeningSort | undefined {
    if (sortOrder === 0) {
      return {
        field: 'date',
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

  private applySelectionFilters(selectedMovieIds: string[], selectedHallIds: string[], selectedDate: string): void {
    const filters: ScreeningFilters = {
      movieId: selectedMovieIds.length > 0 ? selectedMovieIds : undefined,
      hallId: selectedHallIds.length > 0 ? selectedHallIds : undefined,
      date: selectedDate?.trim() || undefined,
    };

    this.facade.applyFilters(filters);
  }

  private currentMovieSelection(): string[] {
    return [...this.facade.movieFilter()];
  }

  private currentHallSelection(): string[] {
    return [...this.facade.hallFilter()];
  }

  private toggleSelection(values: string[], value: string, checked: boolean): string[] {
    if (checked) {
      return values.includes(value) ? values : [...values, value];
    }

    return values.filter((item) => item !== value);
  }
}
