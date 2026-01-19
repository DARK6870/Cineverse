export const identityBasePath = '/identity';

export function isOnIdentityService(): boolean {
  return window.location.pathname.startsWith(identityBasePath);
}

export function isIdentityRoute(path: string): boolean {
  return path.startsWith(identityBasePath);
}

export function needsFullPageReload(targetPath: string): boolean {
  const currentIsIdentity = isOnIdentityService();
  const targetIsIdentity = isIdentityRoute(targetPath);

  return currentIsIdentity !== targetIsIdentity;
}
