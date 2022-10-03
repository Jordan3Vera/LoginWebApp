import { Component, OnInit } from '@angular/core';
import { AutehnticateService } from 'src/app/shared/auth/autehnticate.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  hasChanges() {
    throw new Error('Method not implemented.');
  }

  constructor(private authSvc: AutehnticateService,
              private cookieSvc: CookieService) 
  { }

  // Variables

  ngOnInit(): void {
    this.getUserLogged();
  }

  // Methods 
  getUserLogged(){
    this.authSvc.getUsersApiRest().subscribe(data => {
      let email = this.cookieSvc.get('email');

      let user = Object.values(data);
      user.find(x => {
        if(x.email == email){
          console.log('Datos de usuario: ', x);
        }
      });
    });
  }


}
