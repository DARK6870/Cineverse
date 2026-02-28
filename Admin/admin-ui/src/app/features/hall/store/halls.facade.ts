import { inject, Injectable } from '@angular/core';
import { CreateHallRequestInput, Hall, HallSort, UpdateHallRequestInput } from '../api/hall.graphql.types';
import { HallsStore } from './halls.store';
import { HallGraphqlService } from '../api/hall.graphql.service';

@Injectable({ providedIn: 'root' })
export class HallsFacade {
  private store = inject(HallsStore);
  private hallService = inject(HallGraphqlService);

  readonly halls = this.store.halls;
  readonly totalRecords = this.store.totalRecords;
  readonly pageSize = this.store.pageSize;
  readonly first = this.store.first;
  readonly sort = this.store.sort;
  readonly sortField = this.store.sortField;
  readonly sortOrder = this.store.sortOrder;

  loadPage(first: number, pageSize: number, sort?: HallSort): void {
    void this.loadPageInternal(first, pageSize, sort);
  }

  refresh(): void {
    void this.loadPageInternal(this.first(), this.pageSize(), this.sort());
  }

  async getHallById(id: string): Promise<Hall> {
    return await this.hallService.getHallById(id);
  }

  async createHall(request: CreateHallRequestInput): Promise<void> {
    await this.hallService.createHall(request);
  }

  async updateHall(request: UpdateHallRequestInput): Promise<void> {
    await this.hallService.updateHall(request);
  }

  async deleteHall(id: string): Promise<void> {
    await this.hallService.deleteHall(id);
  }

  private async loadPageInternal(first: number, pageSize: number, sort?: HallSort): Promise<void> {
    const effectiveSort = sort ?? this.sort();
    this.store.setQuery(first, pageSize, effectiveSort);
    const data = await this.hallService.getHallsPage(first, pageSize, effectiveSort);
    this.store.setPageData(data.items ?? [], data.totalCount ?? 0);
  }
}
