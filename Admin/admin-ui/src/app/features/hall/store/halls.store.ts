import { Injectable, computed, signal } from '@angular/core';
import { Hall, HallSort } from '../api/hall.graphql.types';
import { HallsState, initialHallsState } from './halls.state';

@Injectable({ providedIn: 'root' })
export class HallsStore {
  private state = signal<HallsState>(initialHallsState);

  readonly halls = computed(() => this.state().halls);
  readonly totalRecords = computed(() => this.state().totalRecords);
  readonly first = computed(() => this.state().first);
  readonly pageSize = computed(() => this.state().pageSize);
  readonly sort = computed(() => this.state().sort);
  readonly sortField = computed(() => this.state().sort.field);
  readonly sortOrder = computed(() => (this.state().sort.direction === 'ASC' ? 1 : -1));

  setQuery(first: number, pageSize: number, sort: HallSort): void {
    this.state.update((s) => ({ ...s, first, pageSize, sort }));
  }

  setPageData(halls: Hall[], totalRecords: number): void {
    this.state.update((s) => ({
      ...s,
      halls,
      totalRecords,
    }));
  }
}
