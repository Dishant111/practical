import { HttpEvent, HttpHandler, HttpInterceptor, HttpErrorResponse, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor 
{
  constructor(
    private router : Router,
    private toastr: ToastrService,
    private authService :AuthService) {
    
  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    req = req.clone({
      withCredentials : true
    });
  
    return next.handle(req).pipe(catchError((error : HttpErrorResponse) => {
      if (error) {
        if (error.status == 401) {
          
          this.toastr.error("please login in again", "401 error");

          this.authService.reLogin();          
        }  
      }
      return throwError(error);
    }));    
  }
}