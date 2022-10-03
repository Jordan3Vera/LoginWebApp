import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IUser, ILogin, IUserRest } from '../models/interfaces/user.interface';
import { CookieService } from 'ngx-cookie-service';
// API ROUTE 
const API = environment.APIFAKE;
const APIREST = environment.WEBAPIREST;

@Injectable({
  providedIn: 'root'
})
export class AutehnticateService {

  constructor(private http: HttpClient,private cookieSvc: CookieService) { }

  // Cabecera
  header: HttpHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin':'*',
    'scope': '...lo que sea...',
    'Access-Control-Allow-Headers': 'Content-Type',
    'Access-Control-Allow-Methods':'GET,POST,PUT,DELETE'
  });


  // Methods 

  /** Estos métodos pertenecen a la API fake 
   * ****************************************/
  loginJsonServer(user: ILogin): Observable<ILogin>{
    return this.http.post<ILogin>(API + 'login', user, {headers: this.header});
  }

  getUsersJsonServer(): Observable<IUser>{
    let header = new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin':'*',
      'scope': '...lo que sea...',
      'Access-Control-Allow-Headers': 'Content-Type',
      'Access-Control-Allow-Methods':'GET,POST,PUT,DELETE',
      'Authorization': 'Bearer ' + this.cookieSvc.get('token')
    });
    return this.http.get<IUser>(API + 'users', {headers: header})
      .pipe(map((res: any) => {
        return res;
      }));
  }

  getUserJsonServerId(id: IUser): Observable<IUser>{
    return this.http.get<IUser>(API + `users/${id}`);
  }

  postUserJsonServer(user: IUser): Observable<IUser>{
    return this.http.post<IUser>(API + 'users', user, {headers: this.header});
  }

  putUserJsonServer(user: IUser): Observable<IUser>{
    return this.http.put<IUser>(API + `users/${user.id}`, user, {headers: this.header});
  }

  deleteUserJsonServer(id: IUser): Observable<IUser>{
    return this.http.delete<IUser>(API + `users/${id}`);
  }
  /************************************************************************************************** */

  /* Estos métodos pertenecen a la API REST 
  * ****************************************/
  loginApiRest(usuario: any): Observable<any>{
    return this.http.post<any>(APIREST + 'authenticate', usuario, {headers: this.header});
  }

  getUsersApiRest(): Observable<IUserRest>{
    return this.http.get<IUserRest>(APIREST + 'usuarios', {headers: this.header})
      .pipe(map((res: any) => {
        return res;
      }));
  }

  getUserApiRestId(id: any): Observable<any>{
    return this.http.get<any>(APIREST + `usuarios/${id}`, {headers: this.header})
      .pipe(map((res: any) => {
        return res;
      }));
  }

  postUserApiRest(user: any): Observable<IUserRest>{
    return this.http.post<IUserRest>(APIREST + 'usuarios', user, {headers: this.header});
  }

  putPassApiRest(user: any): Observable<Response>{
    return this.http.put<Response>(APIREST + `usuarios/${user.userId}`, user, {headers: this.header});
  }

  deleteUserApiRest(id: number): Observable<any>{
    return this.http.delete<any>(APIREST + `usuario/${id}`);
  }

  //El token
  setToken(token: string){
    this.cookieSvc.set('token',token);
  }

  getToken(){
    return this.cookieSvc.get('token');
  }

  getUserLogged(){
    const token = this.getToken();
    //Aquí iría el endpoint para devolver el usuario para un token
  }
}

