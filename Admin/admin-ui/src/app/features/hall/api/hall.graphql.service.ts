import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import { CreateHallRequestInput, Hall, HallPage, HallSort, UpdateHallRequestInput } from './hall.graphql.types';
import { createHallMutation, deleteHallMutation, getHallByIdQuery, getHallsPageQuery, updateHallMutation } from './hall.graphql';

@Injectable({ providedIn: 'root' })
export class HallGraphqlService {
  private apollo = inject(Apollo);

  public getHallById(id: string): Promise<Hall> {
    return firstValueFrom(
      this.apollo
        .query<{ hallById: Hall }>({
          ...getHallByIdQuery(id),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.hallById)),
    );
  }

  public getHallsPage(skip: number, take: number, sort: HallSort): Promise<HallPage> {
    return firstValueFrom(
      this.apollo
        .query<{ halls: HallPage }>({
          ...getHallsPageQuery(skip, take, sort),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.halls)),
    );
  }

  public async createHall(request: CreateHallRequestInput): Promise<void> {
    await firstValueFrom(
      this.apollo.mutate({
        ...createHallMutation(request),
      }),
    );
  }

  public async updateHall(request: UpdateHallRequestInput): Promise<void> {
    await firstValueFrom(
      this.apollo.mutate({
        ...updateHallMutation(request),
      }),
    );
  }

  public async deleteHall(id: string): Promise<void> {
    await firstValueFrom(
      this.apollo.mutate({
        ...deleteHallMutation(id),
      }),
    );
  }
}
