import { HttpHeaders } from '@angular/common/http';

export class HttpHeaderHelpers {
  static allowAnonymous(): HttpHeaders {
    return new HttpHeaders().set('X-Allow-Anonymous', 'true');
  }

  static withHeaders(headers?: Record<string, string>): HttpHeaders {
    let httpHeaders = new HttpHeaders().set('X-Allow-Anonymous', 'true');

    if (headers) {
      Object.entries(headers).forEach(([key, value]) => {
        httpHeaders = httpHeaders.set(key, value);
      });
    }

    return httpHeaders;
  }
}
