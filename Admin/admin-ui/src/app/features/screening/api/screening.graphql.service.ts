import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import {
  CreateScreeningRequestInput,
  Screening,
  ScreeningFilters,
  ScreeningPage,
  ScreeningSort,
  UpdateScreeningRequestInput,
} from './screening.graphql.types';
import {
  createScreeningMutation,
  deleteScreeningMutation,
  getScreeningByIdQuery,
  getScreeningsPageQuery,
  updateScreeningMutation,
} from './screening.graphql';
import { APOLLO_CLIENTS } from '../../../apollo/apollo.config';

@Injectable({ providedIn: 'root' })
export class ScreeningGraphqlService {
  private apollo = inject(Apollo);
  private cineverseClient = this.apollo.use(APOLLO_CLIENTS.CINEVERSE);

  public getScreeningsPage(
    skip: number,
    take: number,
    sort: ScreeningSort,
    filters: ScreeningFilters,
  ): Promise<ScreeningPage> {
    return firstValueFrom(
      this.cineverseClient
        .query<{ screenings: ScreeningPage }>({
          ...getScreeningsPageQuery(skip, take, sort, filters),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.screenings)),
    );
  }

  public getScreeningById(id: string): Promise<Screening> {
    return firstValueFrom(
      this.cineverseClient
        .query<{ screeningById: Screening }>({
          ...getScreeningByIdQuery(id),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.screeningById)),
    );
  }

  public async createScreening(request: CreateScreeningRequestInput): Promise<void> {
    await firstValueFrom(
      this.cineverseClient.mutate({
        ...createScreeningMutation(request),
      }),
    );
  }

  public async updateScreening(request: UpdateScreeningRequestInput): Promise<void> {
    await firstValueFrom(
      this.cineverseClient.mutate({
        ...updateScreeningMutation(request),
      }),
    );
  }

  public async deleteScreening(id: string): Promise<void> {
    await firstValueFrom(
      this.cineverseClient.mutate({
        ...deleteScreeningMutation(id),
      }),
    );
  }
}
