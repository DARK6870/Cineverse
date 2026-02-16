import { CanDeactivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { Observable } from 'rxjs';

export interface HasUnsavedChanges {
    hasUnsavedChanges(): boolean;
}

export const unsavedChangesGuard: CanDeactivateFn<HasUnsavedChanges> = (
    component,
): Observable<boolean> | boolean => {
    if (!component.hasUnsavedChanges()) {
        return true;
    }

    const confirmationService = inject(ConfirmationService);

    return new Observable<boolean>((observer) => {
        confirmationService.confirm({
            header: 'Unsaved Changes',
            message: 'You have unsaved changes. Are you sure you want to leave this page?',
            rejectButtonProps: { label: 'Stay', severity: 'secondary', outlined: true },
            acceptButtonProps: { label: 'Leave', severity: 'contrast' },
            accept: () => {
                observer.next(true);
                observer.complete();
            },
            reject: () => {
                observer.next(false);
                observer.complete();
            },
        });
    });
};
