import { Component, OnInit, Inject } from '@angular/core';
import { SESSION_STORAGE, StorageService } from 'ngx-webstorage-service';
import { Title } from '@angular/platform-browser';
import { AutehnticateService } from 'src/app/shared/auth/autehnticate.service';
import { map } from 'rxjs';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
  })
  export class HomeComponent implements OnInit {

    constructor(@Inject(SESSION_STORAGE) private storage: StorageService,
                private page: Title)
    {}

    // Variables 

    ngOnInit(){
      this.page.setTitle("Home");
    }
  }









