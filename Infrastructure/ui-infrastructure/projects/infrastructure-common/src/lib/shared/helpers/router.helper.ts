import { inject, Injectable } from '@angular/core';
import { NavigationExtras, Params, Router } from '@angular/router';
import {
  identityBasePath,
  isOnIdentityService,
  needsFullPageReload
} from '../utils/router.utils';

@Injectable({ providedIn: 'root' })
export class RouterHelper {
  private router = inject(Router);

  navigate(path: string, extras?: NavigationExtras): void {
    if (needsFullPageReload(path)) {
      window.location.href = this.buildUrlWithParams(path, extras?.queryParams);
    } else {
      const normalizedPath = this.normalizePathForCurrentApp(path);
      this.router.navigate([normalizedPath], extras).then();
    }
  }

  navigateToIdentity(path: string, extras?: NavigationExtras): void {
    const cleanPath = path.startsWith('/')
      ? path
      : `/${path}`;
    const identityPath = cleanPath.startsWith(identityBasePath)
      ? cleanPath
      : `${identityBasePath}${cleanPath}`;

    window.location.href = this.buildUrlWithParams(
      identityPath,
      extras?.queryParams
    );
  }


  navigateToCineverse(path: string, extras?: NavigationExtras): void {
    if (needsFullPageReload(path)) {
      window.location.href = this.buildUrlWithParams(path, extras?.queryParams);
    } else {
      this.router.navigate([path], extras).then();
    }
  }

  private buildUrlWithParams(path: string, queryParams?: Params | null): string {
    if (!queryParams || Object.keys(queryParams).length === 0) {
      return path;
    }

    const params = new URLSearchParams();
    Object.entries(queryParams).forEach(([key, value]) => {
      if (value != null) {
        params.append(key, String(value));
      }
    });

    const queryString = params.toString();
    return queryString ? `${path}?${queryString}` : path;
  }

  private normalizePathForCurrentApp(path: string): string {
    if (!isOnIdentityService()) {
      return path;
    }

    if (!path.startsWith(identityBasePath)) {
      return path;
    }

    const localPath = path.slice(identityBasePath.length);
    return localPath.length > 0 ? localPath : '/';
  }
}
