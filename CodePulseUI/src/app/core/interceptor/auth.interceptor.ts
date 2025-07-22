import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private cookieService: CookieService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this.isInterceptReq(request)) {
      const authReq = request.clone({
        setHeaders: {
          'Authorization': this.cookieService.get('Authorization')
        }
      });
      return next.handle(authReq);
    }

    return next.handle(request);

  }

  private isInterceptReq(req: HttpRequest<any>): boolean {
    return req.urlWithParams.indexOf('addAuth=true', 0) > -1 ? true : false;
  }
}
