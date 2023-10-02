import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from "@angular/core"
import { Observable, of } from "rxjs";
import { tap, catchError } from "rxjs/operators";

@Injectable()
export class GoogleAuthInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request)
      .pipe(
        catchError((error: any) => {
          
          if (error instanceof HttpErrorResponse && error.status === 0) {
            window.location.href = '/Account/Login';
          }

          throw new Error(error?.message ?? 'Unknown error');
        })
      )
  }

}
