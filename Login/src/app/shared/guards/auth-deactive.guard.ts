import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { DashboardComponent } from '../../global/views/dashboard/dashboard.component';

@Injectable({
  providedIn: 'root'
})
export class AuthDeactiveGuard implements CanDeactivate<DashboardComponent> {
  canDeactivate(
    component: DashboardComponent,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // if(component.hasChanges()){
    //   return window.confirm('Do you really want to exit?');
    // }
    return true;
  }
  
}
