import { Injectable } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { map, Observable } from 'rxjs';
import { GET_MOVIES_QUERY } from './movie.operations';
import { Movie } from '../../utils/types/api/movie';

@Injectable({
  providedIn: 'root'
})

export class MovieGraphqlService {
  constructor(private apollo: Apollo) {}

  public getMovies(ids: string[]): Observable<Movie[]> {
    return this.apollo.watchQuery({
      query: GET_MOVIES_QUERY,
      variables: {
        ids: ids
      },
      context: {
        allowAnonymous: true
      }
    }).valueChanges.pipe(
      map((result: any) => result.data.movies.items)
    );
  }
}
