import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private route: Router, private toastr: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                const modelStatueErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStatueErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStatueErrors.flat();
              } else if(typeof(error.error) === 'object') {
                this.toastr.error('Bad Request', error.status);
              }else{
                this.toastr.error(error.error,error.state);
              }
              break;
            case 401:
              this.toastr.error('Unauthorized', error.status);
              break;
            case 404:
              this.route.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error:error.error}}
              this.route.navigateByUrl('/server-error',navigationExtras);
              break;
            default:
              this.toastr.error('Something unexected went wrong');
              break;
          }
        }
        return throwError(error);
      })
    )
  }
}
