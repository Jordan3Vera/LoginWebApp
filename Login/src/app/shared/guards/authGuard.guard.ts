import { Injectable, Inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { SESSION_STORAGE, StorageService } from 'ngx-webstorage-service';
import { CookieService } from 'ngx-cookie-service';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(@Inject(SESSION_STORAGE) private storage: StorageService,
              private router: Router,
              private cookieSvc: CookieService)
  {}

  /**En caso de que se quiera en todas las páginas hijas de una ruta estén protegidas lo que se tiene que hacer 
   * es cambiar un par de cosas en el guard. El guard ahora no implementará de CanActivate sino de CanActivateChild, 
   * y el método dentro de la clase, como es obvio, también cambiará a CanActivateChild(). El resto de las cosas 
   * del guard se mantiene.
   */

  /**Para usar este guard para todos los hijos se haría lo mismo que antes (añadiendo el guard en el componente padre), 
   * añadiendo a la ruta, pero este vez el campo de la ruta se llamaría canActivateChild
   */

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if(!this.cookieSvc.get('token') && !this.storage.get('isLoggedIn')){
      Swal.fire({
        icon: 'error',
        title: 'Advertencia',
        html: `<span class="text-xl">Página no encontrada 😣</span>`,
        // text: 'Page not found',
        showConfirmButton: false,
        timer: 2000
      });
      this.router.navigate(['/']);
      return false;
    }
    return true;
  }
  
}
