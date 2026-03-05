import { inject, Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { firstValueFrom, map } from 'rxjs';
import { APOLLO_CLIENTS } from '../../../apollo/apollo.config';
import { User, UserDistinctField, UserFilters, UserPage, UserSort, UpdateUserRequestInput } from './user.graphql.types';
import { getUserByIdQuery, getUsersDistinctFilterValuesQuery, getUsersPageQuery, updateUserMutation } from './user.graphql';

@Injectable({ providedIn: 'root' })
export class UserGraphqlService {
  private apollo = inject(Apollo);
  private identityClient = this.apollo.use(APOLLO_CLIENTS.IDENTITY);

  public getUserById(id: string): Promise<User> {
    return firstValueFrom(
      this.identityClient
        .query<{ userById: User }>({
          ...getUserByIdQuery(id),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data!.userById)),
    );
  }

  public getUsersPage(
    skip: number,
    take: number,
    sort: UserSort,
    filters: UserFilters,
    searchTerm?: string,
  ): Promise<UserPage> {
    return firstValueFrom(
      this.identityClient
        .query<{ users: UserPage }>({
          ...getUsersPageQuery(skip, take, sort, filters, searchTerm),
          fetchPolicy: 'network-only',
        })
        .pipe(
          map((res) => ({
            items: res.data?.users?.items ?? [],
            totalCount: res.data?.users?.totalCount ?? 0,
          })),
        ),
    );
  }

  public getUsersDistinctFilterValues(field: UserDistinctField): Promise<string[]> {
    return firstValueFrom(
      this.identityClient
        .query<{ usersDistinctFilterValues: string[] }>({
          ...getUsersDistinctFilterValuesQuery(field),
          fetchPolicy: 'network-only',
        })
        .pipe(map((res) => res.data?.usersDistinctFilterValues ?? [])),
    );
  }

  public async updateUser(request: UpdateUserRequestInput): Promise<void> {
    await firstValueFrom(
      this.identityClient.mutate({
        ...updateUserMutation(request),
      }),
    );
  }
}
