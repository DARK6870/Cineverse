import { ErrorLink } from '@apollo/client/link/error';
import { ToastService } from '../../toast/services/toast.service';
import { inject } from '@angular/core';

// TODO: check
export function createErrorLink() {
  const toastService = inject(ToastService);

  return new ErrorLink(({ error }) => {
    if (error.message) {
        toastService.error(error.message);
        console.error(error.message);
    }
  });
}
