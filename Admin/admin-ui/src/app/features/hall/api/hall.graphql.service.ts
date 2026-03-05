import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import { CreateHallRequestInput, Hall, HallPage, HallSort, UpdateHallRequestInput } from './hall.graphql.types';
import { createHallMutation, deleteHallMutation, getHallByIdQuery, getHallsPageQuery, updateHallMutation } from './hall.graphql';
import { APOLLO_CLIENTS } from '../../../apollo/apollo.config';

@Injectable({ providedIn: 'root' })
export class HallGraphqlService {
  private apollo = inject(Apollo);
  private cineverseClient = this.apollo.use(APOLLO_CLIENTS.CINEVERSE);

  public getHallById(id: string): Promise<Hall> {
    return firstValueFrom(
      this.cineverseClient
        .query<{ hallById: Hall }>({
          ...getHallByIdQuery(id),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.hallById)),
    );
  }

  public getHallsPage(skip: number, take: number, sort: HallSort): Promise<HallPage> {
    return firstValueFrom(
      this.cineverseClient
        .query<{ halls: HallPage }>({
          ...getHallsPageQuery(skip, take, sort),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.halls)),
    );
  }

  public async createHall(request: CreateHallRequestInput): Promise<void> {
    await firstValueFrom(
      this.cineverseClient.mutate({
        ...createHallMutation(request),
      }),
    );
  }

  public async updateHall(request: UpdateHallRequestInput): Promise<void> {
    await firstValueFrom(
      this.cineverseClient.mutate({
        ...updateHallMutation(request),
      }),
    );
  }

  public async deleteHall(id: string): Promise<void> {
    await firstValueFrom(
      this.cineverseClient.mutate({
        ...deleteHallMutation(id),
      }),
    );
  }
}
