import { Component, Inject, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { SESSION_STORAGE, StorageService } from 'ngx-webstorage-service';
import { Router } from '@angular/router';
import { AutehnticateService } from '../../../../shared/auth/autehnticate.service';

@Component({
  selector: 'app-log-up',
  templateUrl: './log-up.component.html',
  styleUrls: ['./log-up.component.scss']
})
export class LogUpComponent implements OnInit {

  constructor(@Inject(SESSION_STORAGE) private storage: StorageService,
              private router: Router,
              private authSvc: AutehnticateService) 
  { }

  isLoggedIn: boolean = false;
  registerForm: any = new FormGroup({
    username: new FormControl('',Validators.required),
    email: new FormControl('',Validators.required),
    password: new FormControl('',Validators.required),
    confirmPassword: new FormControl('',Validators.required)
  });
  users: any[] = [];
  checked: boolean = false;

  ngOnInit(): void {
    this.storage.set('isLoggedIn',this.isLoggedIn);
    this.authSvc.getUsersApiRest().subscribe(data=>console.log(data));
    
  }

  Register(){

    // Objeto a guardar valor 
    const user: any = {
      username: this.registerForm.value.username,
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword
    }

    // Verificación si existe 
    let verifyEmail = this.registerForm.value.email;
    let verifyUsername = this.registerForm.value.username;
    let resEmail: boolean, resUsername: boolean = false;

    if(this.registerForm.value.username != '' || this.registerForm.value.email != '' || 
       this.registerForm.value.password != '' || this.registerForm.value.confirmPassword != '')
      {
        this.authSvc.getUsersApiRest().subscribe((data: any) => {
          this.users = [];
          this.users = data;

          // Consulto si el valor ingresado ya existe en el servidor 
          for(let user of this.users){
            if(user.email === verifyEmail){
              resEmail = true;
            }

            if(user.userName === verifyUsername){
              resUsername = true;
            }
          }

          // Respuestas
          if(resUsername){
            Swal.fire({
              icon: 'error',
              title: 'Error de registro',
              text: 'Este usuario ya está en uso, elija otro...',
              confirmButtonText: 'Aceptar',
              allowOutsideClick: false
            }).then((res) => {
              if(res.value){
                this.registerForm.reset();
              }
            });
          }

          if(resEmail){
            Swal.fire({
              icon: 'error',
              title: 'Error de registro',
              text: 'Este correo ya está en uso, elija otro...',
              confirmButtonText: 'Aceptar',
              allowOutsideClick: false
            }).then((res) => {
              if(res.value){
                this.registerForm.reset();
              }
            });
          }

          // else{
          if(this.registerForm.value.password != this.registerForm.value.confirmPassword){
            Swal.fire({
              icon: 'info',
              title: 'Advertencia',
              text: 'Las contraseñas no coinciden',
              showConfirmButton: false,
              timer: 2000
            });
          }else{
            this.authSvc.postUserApiRest(user).subscribe({
              next: () => {
                Swal.fire({
                  icon: 'success',
                  title: 'Exito',
                  text: 'El registro fue exitoso',
                  showConfirmButton: false,
                  timer: 2000
                }).then((res) => {
                  if(res){
                    this.router.navigate(['/']);
                  }
                })
              },
              error: (err) => {
                throw new Error("Error" + err);
              }
            })
          }
        });
      }else{
        Swal.fire({
          icon: 'info',
          title: 'Advertencia',
          text: 'Las contraseñas son distintas',
          showConfirmButton: false,
          timer: 2000
        });
    }
  }

}
