import { inject } from '@angular/core';
import { CanActivateChildFn } from '@angular/router';
import { AuthenticationService, authenticationGuard } from '@cineverse/infrastructure-auth';
import { RouterHelper, ToastService, identityBasePath } from '@cineverse/infrastructure-common';

const ALLOWED_ROLES = new Set(['Admin', 'Manager']);

function normalizeRoles(role: unknown): string[] {
  if (Array.isArray(role)) {
    return role.filter((value): value is string => typeof value === 'string');
  }

  if (typeof role === 'string') {
    return role
      .split(',')
      .map((value) => value.trim())
      .filter((value) => value.length > 0);
  }

  return [];
}

export const adminManagerGuard: CanActivateChildFn = async (route, state) => {
  const isAuthenticated = authenticationGuard(route, state);
  if (!isAuthenticated) {
    return false;
  }

  const authenticationService = inject(AuthenticationService);
  const toastService = inject(ToastService);
  const routerHelper = inject(RouterHelper);

  try {
    const userData = await authenticationService.getUserDataAsync();
    const roles = normalizeRoles(userData.role);
    const isAllowed = roles.some((role) => ALLOWED_ROLES.has(role));

    if (isAllowed) {
      return true;
    }

    toastService.warning('Access denied. Only Admin and Manager roles can open admin-ui.');
    routerHelper.navigate(`${identityBasePath}/profile`);
    return false;
  } catch {
    return false;
  }
};
