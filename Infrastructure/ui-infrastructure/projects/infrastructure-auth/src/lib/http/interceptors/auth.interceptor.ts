import { HttpInterceptorFn } from '@angular/common/http';
import { inject, Injector } from '@angular/core';
import { from, switchMap } from 'rxjs';
import { AuthenticationService } from '../../services/authentication/authentication.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const injector = inject(Injector);

  const allowAnonymous = req.headers.has('X-Allow-Anonymous');

  if (allowAnonymous) {
    return next(req.clone({
      headers: req.headers.delete('X-Allow-Anonymous')
    }));
  }

  const authenticationService = injector.get(AuthenticationService);

  return from(authenticationService.getOrGenerateAccessTokenAsync()).pipe(
    switchMap(token => {
      if (token) {
        const authReq = req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        });
        return next(authReq);
      }

      return next(req);
    })
  );
};
