import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { LoadingService } from '../../../feedback/loading/services/loading.service';
import { BlockActionsService } from '../../../feedback/blocking/services/block-actions.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  const blockActionsService = inject(BlockActionsService);

  const isGetRequest = req.method === 'GET';

  if (isGetRequest) {
    loadingService.show();
  } else {
    blockActionsService.block();
  }

  return next(req).pipe(
    finalize(() => {
      if (isGetRequest) {
        loadingService.hide();
      } else {
        blockActionsService.unblock();
      }
    })
  );
};
