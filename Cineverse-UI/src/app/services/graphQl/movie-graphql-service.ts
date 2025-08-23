import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { GET_MOVIES_QUERY } from '../../common/constants/graphql/movie-operations';
import { Movie } from '../../common/models/movie';

@Injectable({
  providedIn: 'root'
})

export class MovieGraphQlService {
  constructor(private apollo: Apollo) {}

  getMovies(ids: string[]): Observable<Movie[]> {
    return this.apollo.watchQuery({
      query: GET_MOVIES_QUERY,
      variables: {
        ids: ids
      }
    }).valueChanges.pipe(
      map((result: any) => result.data.movies.items),
    );
  }
}
