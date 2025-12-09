import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Hall } from './hall.graphql.types';
import { getHallByIdQuery } from './hall.graphql';
import { Apollo } from 'apollo-angular';

@Injectable({ providedIn: 'root' })
export class HallGraphqlService{
  private apollo = inject(Apollo);

  public getHallById(id: string) : Observable<Hall>{
    return this.apollo
      .query<{ hallById: Hall }>({
        ...getHallByIdQuery(id)
      }).pipe(map((res) => res.data.hallById));
  }
}
