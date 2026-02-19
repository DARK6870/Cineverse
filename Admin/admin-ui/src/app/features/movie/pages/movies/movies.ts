import { CommonModule } from '@angular/common';
import { Component, DestroyRef, HostListener, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { ButtonDirective } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { filter } from 'rxjs/operators';
import { MovieFilters, MovieSort } from '../../api/movie.graphql.types';
import { MoviesFacade } from '../../store/movies.facade';

@Component({
  selector: 'app-movies',
  imports: [CommonModule, TableModule, ButtonDirective, RouterLink, TagModule],
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
  protected readonly genreFilterOptions = [
    'Action',
    'Adventure',
    'Animation',
    'Comedy',
    'Crime',
    'Documentary',
    'Drama',
    'Family',
    'Fantasy',
    'Horror',
    'Romance',
    'Sci-Fi',
    'Thriller',
  ];

  protected facade = inject(MoviesFacade);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  protected isGenreFilterOpen = false;
  protected isAvailabilityFilterOpen = false;
  protected selectedGenres: string[] = [];
  protected selectedAvailability: Array<(typeof Movies.AVAILABILITY_VALUES)[number]> = [];
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
    this.selectedGenres = [...this.facade.genreFilter()];
    this.selectedAvailability = [...this.facade.availabilityFilter()] as Array<(typeof Movies.AVAILABILITY_VALUES)[number]>;

    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(() => {
        if (this.router.url.startsWith('/movies') && history.state?.refresh) {
          this.facade.refresh();
        }
      });
  }

  onLazyLoad(event: TableLazyLoadEvent): void {
    const first = event.first ?? this.facade.first();
    const pageSize = event.rows ?? this.facade.pageSize();
    const sort = this.getSortFromEvent(event.sortField, event.sortOrder);
    this.facade.loadPage(first, pageSize, sort);
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
    if (checked) {
      if (!this.selectedGenres.includes(genre)) {
        this.selectedGenres = [...this.selectedGenres, genre];
      }
      return;
    }

    this.selectedGenres = this.selectedGenres.filter((value) => value !== genre);
  }

  protected toggleAvailabilitySelection(
    value: (typeof Movies.AVAILABILITY_VALUES)[number],
    checked: boolean,
  ): void {
    if (checked) {
      if (!this.selectedAvailability.includes(value)) {
        this.selectedAvailability = [...this.selectedAvailability, value];
      }
      return;
    }

    this.selectedAvailability = this.selectedAvailability.filter((currentValue) => currentValue !== value);
  }

  protected applyGenreFilters(): void {
    this.applyFilters();
    this.isGenreFilterOpen = false;
  }

  protected applyAvailabilityFilters(): void {
    this.applyFilters();
    this.isAvailabilityFilterOpen = false;
  }

  protected clearGenreFilters(): void {
    this.selectedGenres = [];
    this.closeAllFilterMenus();
  }

  protected clearAvailabilityFilters(): void {
    this.selectedAvailability = [];
    this.closeAllFilterMenus();
  }

  protected isGenreSelected(genre: string): boolean {
    return this.selectedGenres.includes(genre);
  }

  protected isAvailabilitySelected(value: (typeof Movies.AVAILABILITY_VALUES)[number]): boolean {
    return this.selectedAvailability.includes(value);
  }

  protected genreFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Genre', this.selectedGenres.length);
  }

  protected availabilityFilterButtonLabel(): string {
    return this.getFilterButtonLabel('Availability', this.selectedAvailability.length);
  }

  protected filteredGenreOptions(): string[] {
    const normalizedSearch = this.genreSearchTerm.trim().toLowerCase();
    if (!normalizedSearch) {
      return this.genreFilterOptions;
    }

    return this.genreFilterOptions.filter((genre) => genre.toLowerCase().includes(normalizedSearch));
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

  private applyFilters(): void {
    this.facade.applyFilters({
      genre: this.selectedGenres.length > 0 ? this.selectedGenres : undefined,
      isAvailable: this.parseAvailabilityValues(this.selectedAvailability),
    });
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
}
