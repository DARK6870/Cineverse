import { CommonModule, NgOptimizedImage } from '@angular/common';
import { Component, HostListener, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { ButtonDirective } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { MovieFilters, MovieSort } from '../../api/movie.graphql.types';
import { MoviesFacade } from '../../store/movies.facade';

@Component({
  selector: 'app-movies',
  imports: [CommonModule, TableModule, ButtonDirective, RouterLink, TagModule, NgOptimizedImage],
  templateUrl: 'movies.html',
  styleUrl: 'movies.css',
  standalone: true,

})
export class Movies implements OnInit {
  private static readonly AVAILABILITY_VALUES = ['available', 'unavailable'] as const;

  protected readonly availabilityFilterOptions = [
    { label: 'Available', value: 'available' as const },
    { label: 'Unavailable', value: 'unavailable' as const },
  ];

  protected facade = inject(MoviesFacade);

  protected isGenreFilterOpen = false;
  protected isAvailabilityFilterOpen = false;
  protected genreSearchTerm = '';
  protected availabilitySearchTerm = '';

  tableHeight = 'calc(100vh - 300px)';
  private readonly sortableColumns: Record<string, MovieSort['field']> = {
    title: 'title',
    genre: 'genre',
    releaseDate: 'releaseDate',
    duration: 'duration',
    isAvailable: 'isAvailable',
  };

  ngOnInit() {
    this.facade.loadGenreFilterOptions();
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

  protected toggleGenreFilterMenu(): void {
    this.isGenreFilterOpen = !this.isGenreFilterOpen;
    if (this.isGenreFilterOpen) {
      this.isAvailabilityFilterOpen = false;
    }
  }

  protected toggleAvailabilityFilterMenu(): void {
    this.isAvailabilityFilterOpen = !this.isAvailabilityFilterOpen;
    if (this.isAvailabilityFilterOpen) {
      this.isGenreFilterOpen = false;
    }
  }

  protected toggleGenreSelection(genre: string, checked: boolean): void {
    const selectedGenres = [...this.facade.genreFilter()];
    if (checked) {
      if (!selectedGenres.includes(genre)) {
        selectedGenres.push(genre);
      }
    } else {
      const index = selectedGenres.indexOf(genre);
      if (index >= 0) {
        selectedGenres.splice(index, 1);
      }
    }

    this.facade.applyFilters({
      genre: selectedGenres.length > 0 ? selectedGenres : undefined,
      isAvailable: this.parseAvailabilityValues(this.currentAvailabilitySelection()),
    });
  }

  protected toggleAvailabilitySelection(
    value: (typeof Movies.AVAILABILITY_VALUES)[number],
    checked: boolean,
  ): void {
    const selectedAvailability = [...this.currentAvailabilitySelection()];
    if (checked) {
      if (!selectedAvailability.includes(value)) {
        selectedAvailability.push(value);
      }
    } else {
      const index = selectedAvailability.indexOf(value);
      if (index >= 0) {
        selectedAvailability.splice(index, 1);
      }
    }

    this.facade.applyFilters({
      genre: this.facade.genreFilter().length > 0 ? this.facade.genreFilter() : undefined,
      isAvailable: this.parseAvailabilityValues(selectedAvailability),
    });
  }

  protected applyGenreFilters(): void {
    this.isGenreFilterOpen = false;
  }

  protected applyAvailabilityFilters(): void {
    this.isAvailabilityFilterOpen = false;
  }

  protected clearGenreFilters(): void {
    this.facade.applyFilters({
      genre: undefined,
      isAvailable: this.parseAvailabilityValues(this.currentAvailabilitySelection()),
    });
    this.closeAllFilterMenus();
  }

  protected clearAvailabilityFilters(): void {
    this.facade.applyFilters({
      genre: this.facade.genreFilter().length > 0 ? this.facade.genreFilter() : undefined,
      isAvailable: undefined,
    });
    this.closeAllFilterMenus();
  }

  protected isGenreSelected(genre: string): boolean {
    return this.facade.genreFilter().includes(genre);
  }

  protected isAvailabilitySelected(value: (typeof Movies.AVAILABILITY_VALUES)[number]): boolean {
    return this.facade.availabilityFilter().includes(value);
  }

  protected genreFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Genre', this.facade.genreFilter().length);
  }

  protected availabilityFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Availability', this.facade.availabilityFilter().length);
  }

  protected filteredGenreOptions(): string[] {
    const normalizedSearch = this.genreSearchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return this.facade.genreFilterOptions();
    }

    return this.facade.genreFilterOptions().filter((genre) => genre.toLowerCase().includes(normalizedSearch));
  }

  protected filteredAvailabilityOptions(): typeof this.availabilityFilterOptions {
    const normalizedSearch = this.availabilitySearchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return this.availabilityFilterOptions;
    }

    return this.availabilityFilterOptions.filter((availabilityOption) =>
      availabilityOption.label.toLowerCase().includes(normalizedSearch),
    );
  }

  @HostListener('document:click', ['$event'])
  protected onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement | null;
    if (target?.closest('.filter-menu')) {
      return;
    }

    this.closeAllFilterMenus();
  }

  protected highlightSearchTerm(value: string | null | undefined): string {
    const safeValue = this.escapeHtml(value ?? '');
    const activeSearchTerm = this.facade.searchTerm().trim();
    if (!activeSearchTerm) {
      return safeValue;
    }

    const searchRegex = new RegExp(`(${this.escapeRegExp(activeSearchTerm)})`, 'gi');
    return safeValue.replace(searchRegex, '<mark class="movie-search-highlight">$1</mark>');
  }

  private closeAllFilterMenus(): void {
    this.isGenreFilterOpen = false;
    this.isAvailabilityFilterOpen = false;
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
  ): MovieSort | undefined {
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

  private parseAvailabilityValues(
    availabilityValues: Array<(typeof Movies.AVAILABILITY_VALUES)[number]>,
  ): MovieFilters['isAvailable'] {
    const values = availabilityValues
      .map((value) => (value === 'available' ? true : false))
      .filter((value, index, allValues) => allValues.indexOf(value) === index);

    if (values.length === 0 || values.length === 2) {
      return undefined;
    }

    return values;
  }

  private currentAvailabilitySelection(): Array<(typeof Movies.AVAILABILITY_VALUES)[number]> {
    return this.facade
      .availabilityFilter()
      .filter(
        (value): value is (typeof Movies.AVAILABILITY_VALUES)[number] =>
          Movies.AVAILABILITY_VALUES.includes(value as (typeof Movies.AVAILABILITY_VALUES)[number]),
      );
  }

  private escapeRegExp(value: string): string {
    return value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  }

  private escapeHtml(value: string): string {
    return value
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  }
}
