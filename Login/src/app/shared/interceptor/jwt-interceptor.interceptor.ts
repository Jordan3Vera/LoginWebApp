import { Inject, Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, ReplaySubject, map, throwError, catchError, finalize} from 'rxjs';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { SESSION_STORAGE, StorageService } from 'ngx-webstorage-service';

@Injectable()
export class JwtInterceptorInterceptor implements HttpInterceptor {

  private pendingRequests = 0;
  private filteredUrlPatterns: RegExp[] = [];
  private pendingRequestsStatus: ReplaySubject<boolean> = new ReplaySubject<boolean>(1);

  constructor(@Inject(SESSION_STORAGE) private storage: StorageService ,private router: Router) {
    router.events.subscribe((event) => {
      if(event instanceof NavigationStart){
        this.pendingRequestsStatus.next(true);
      }

      if((event instanceof NavigationError) || event instanceof NavigationEnd || event instanceof NavigationCancel){
        this.pendingRequestsStatus.next(false);
      }
    });
  }

  private shouldBypass(url: string): boolean {
    return this.filteredUrlPatterns.some((e) => {
      return e.test(url);
    });
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const shouldBypass = this.shouldBypass(request.url);
    const token: string = this.storage.get("token");

    if(token){
      request = request.clone({
        headers: request.headers.set("Authorization", "Bearer " + token)
      });
    }

    if(!request.headers.has('Content-Type')){
      request = request.clone({
        headers: request.headers.set('Content-Type','application/json')
      });
    }

    request = request.clone({headers: request.headers.set('Accept','application/json')});

    if(!shouldBypass){
      this.pendingRequests++;
      if(1 === this.pendingRequests){
        this.pendingRequestsStatus.next(true);
      }
    }

    return next.handle(request).pipe(
      map((event) => {
        return event;
      }),
      catchError((err: any) => {
        return throwError(err);
      }),
      finalize(() => {
        if(!shouldBypass){
          this.pendingRequests--;

          if(0 === this.pendingRequests){
            this.pendingRequestsStatus.next(false);
          }
        }
      })
    );
  }
}
