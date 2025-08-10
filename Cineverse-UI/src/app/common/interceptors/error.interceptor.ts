import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { catchError } from 'rxjs/operators';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const messageService = inject(MessageService);

  return next(req).pipe(
    catchError((error) => {
      let errorMessage = 'Invalid Request';

      if (error.error?.errors?.[0]?.message) {
        errorMessage = error.error.errors[0].message;
      } else if (error.error?.message) {
        errorMessage = error.error.message;
      } else if (error.message) {
        errorMessage = error.message;
      }

      messageService.add({
        severity: 'error',
        summary: 'Request Error',
        detail: errorMessage
      });

      throw error;
    })
  );
};
