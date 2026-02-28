import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ButtonDirective } from 'primeng/button';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { Hall, HallSort } from '../../api/hall.graphql.types';
import { HallsFacade } from '../../store/halls.facade';

@Component({
  selector: 'app-halls',
  imports: [CommonModule, TableModule, ButtonDirective, RouterLink],
  templateUrl: 'halls.html',
  styleUrl: 'halls.css',
  standalone: true,
})
export class Halls {
  protected facade = inject(HallsFacade);

  tableHeight = 'calc(100vh - 300px)';

  private readonly sortableColumns: Record<string, HallSort['field']> = {
    name: 'name',
  };

  onLazyLoad(event: TableLazyLoadEvent): void {
    const first = event.first ?? this.facade.first();
    const pageSize = event.rows ?? this.facade.pageSize();
    const sort = this.getSortFromEvent(event.sortField, event.sortOrder);
    this.facade.loadPage(first, pageSize, sort);
  }

  protected seatsCount(hall: Hall): number {
    return hall.seats?.length ?? 0;
  }

  protected rowCount(hall: Hall): number {
    if (!hall.seats?.length) {
      return 0;
    }

    return new Set(hall.seats.map((seat) => seat.row)).size;
  }

  private getSortFromEvent(
    sortField: string | string[] | null | undefined,
    sortOrder: number | null | undefined,
  ): HallSort | undefined {
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
