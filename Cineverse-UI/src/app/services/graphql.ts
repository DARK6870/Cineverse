import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GraphqlService {
  constructor(private apollo: Apollo) {}

  testQuery(): Observable<any> {
    return this.apollo.query({
      query: gql`
        query TestQuery {
          __schema {
            types {
              name
              kind
            }
          }
        }
      `
    });
  }
}
