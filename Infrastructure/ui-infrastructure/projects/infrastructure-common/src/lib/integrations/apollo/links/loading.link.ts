import { ApolloLink, Observable } from '@apollo/client/core';
import { inject } from '@angular/core';
import { LoadingService } from '../../../feedback/loading/services/loading.service';
import { BlockActionsService } from '../../../feedback/blocking/services/block-actions.service';

export function createLoadingLink(): ApolloLink {
  const loadingService = inject(LoadingService);
  const blockActionsService = inject(BlockActionsService);

  return new ApolloLink((operation, forward) => {
    const isQuery = operation.query.definitions.some(
      (def: any) =>
        def.kind === 'OperationDefinition' && def.operation === 'query'
    );

    if (isQuery) loadingService.show();
    else blockActionsService.block();

    const finish = () => {
      if (isQuery) loadingService.hide();
      else blockActionsService.unblock();
    };

    return new Observable(observer => {
      const sub = forward(operation).subscribe({
        next: result => observer.next(result),
        error: error => {
          finish();
          observer.error(error);
        },
        complete: () => {
          finish();
          observer.complete();
        },
      });

      return () => {
        sub.unsubscribe();
        finish();
      };
    });
  });
}
