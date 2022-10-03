import { Component, OnInit, Inject } from '@angular/core';
import { MenuItem } from 'primeng/api';
import Swal from 'sweetalert2';
import { SESSION_STORAGE, StorageService } from 'ngx-webstorage-service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import * as $ from 'jquery';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor(@Inject(SESSION_STORAGE) private storage: StorageService,
              private router: Router,
              private cookie: CookieService) 
  { }

  items: MenuItem[] = [];
  itemUser: MenuItem[] = [];
  usereadonly: string = this.storage.get('user');

  ngOnInit(): void {
    this.items = [
      { label: 'Inicio', icon: 'pi pi-home'},
      { label: 'Lista', icon: 'fa-solid fa-file-word'},
      { 
        label: this.usereadonly,
        icon: 'pi pi-fw pi-user',
        id: 'respSession',
        items: [
          {
            icon: 'fa-regular fa-id-badge',
            label: 'Perfil'
          },
          {
            icon: 'pi pi-cog',
            label: 'Configuración'
          },
          { separator: true}
        ]
      },
      { label: 'Cerrar sesión', icon: 'pi pi-fw pi-power-off', id: 'respSession', command: () => {this.buttonCloseSession()}}
    ];

    this.itemUser = [
      {
        icon: 'fa-regular fa-id-badge',
        label: 'Perfil',
      },
      {
        label: 'Configuración',
        icon: 'pi pi-cog'
      },
      {
        separator: true
      },
      {
        icon: 'pi pi-fw pi-power-off',
        label: 'Cerrar sesión',
        styleClass: 'iconCloseButton',
        command: () => {this.buttonCloseSession()}
      }
    ];
    
  }

  // Methods 
  buttonCloseSession(){ //Este es para cerrar la sesión 
    Swal.fire({
      title: '¿Está seguro que quieres cerrar sesión?',
      showDenyButton: true,
      confirmButtonText: 'Si',
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire({
          title: '¡Sesión cerrada!',
          icon: 'success',
          text: 'No dudes en regresar',
          timer: 2000,
          showConfirmButton: false,
        }).then((result) => {
          if(result){
            this.storage.remove("isLogguedIn");
            this.cookie.delete("token");
            this.router.navigate(['/']);
          }
        });
      } 
    });
  }

}
