import { ErrorLink } from '@apollo/client/link/error';
import { inject } from '@angular/core';
import { ToastService } from '../../../feedback/toast/services/toast.service';

// TODO: fix
export function createErrorLink() {
  const toastService = inject(ToastService);

  return new ErrorLink(({ error }) => {
    if (error.message) {
        toastService.error(error.message);
        console.error('GraphQL Error', error.message, error);
    }
  });
}
