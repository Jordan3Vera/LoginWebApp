import { Component, Inject, Input, OnInit } from '@angular/core';
import { AutehnticateService } from '../../../../shared/auth/autehnticate.service';
import { find, first, map } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SESSION_STORAGE, StorageService } from 'ngx-webstorage-service';
import { ILogin, IUser } from '../../../../shared/models/interfaces/user.interface';
import Swal from 'sweetalert2';
import { Title } from '@angular/platform-browser';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.scss']
})
export class LogInComponent implements OnInit {

  constructor(@Inject(SESSION_STORAGE) private storage: StorageService,
              private authSvc: AutehnticateService,
              private router: Router,
              private page: Title,
              private cookie: CookieService) 
  { }

  // Vars 
  isLoggedIn: boolean = false;
  loginForm: any = new FormGroup({
    email: new FormControl('',Validators.required),
    password: new FormControl('',Validators.required)
  });
  checked: boolean = false;
  /*********************************************** */
  dialogFIS: boolean = false; //Este dialogo es para registrar un usuario en FIS
  dialogEmailConfirmFIS: boolean = false;
  dialogPassResetFIS: boolean = false;
  // formPassFIS: boolean = false;
  emailFormFIS: any = new FormGroup({
    email: new FormControl('',Validators.required)
  });
  resetFormFIS: any = new FormGroup({
    password: new FormControl('',Validators.required),
    confirmPassword: new FormControl('',Validators.required)
  });

  ngOnInit(): void {
    // this.getUsers();
    this.page.setTitle("Iniciar sesión");
    this.storage.set("isLoggedIn", this.isLoggedIn);
    this.storage.remove('user');
    this.storage.remove('email');
    this.cookie.delete('token');
    // this.dialogFIS = false;
    this.cookie.delete('id');

    this.authSvc.getUsersJsonServer().subscribe(data => {
      console.log(data);
    })
  }

  // Methods 
  getUsers(){
    this.authSvc.getUsersJsonServer().pipe(map((data: any) => {console.log("Usuarios json-server:", data)})).subscribe();
    this.authSvc.getUsersApiRest().pipe(map((data: any) => {console.log("Usuarios api-rest:", data)})).subscribe();
  }

  // Este login es de la APIFAKE
  loginApiFake(){
    
    const user: ILogin = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };
    
    // API FAKE 
    if(user){
      this.authSvc.loginJsonServer(user)
      .pipe(first())
      .subscribe({
        next: (data) => {
          if(data){
            Swal.fire({
              icon: 'success',
              title: 'Logueo exitoso <span style="color: green; font-size:2em;">☺</span>',
              text: 'Los datos son correctos',
              showConfirmButton: false,
              timer: 2000
            }).then((res) => {
              if(res){
                this.storage.set("isLoggedIn", true);
                this.cookie.set('token',data.accessToken);
                this.storage.set('user',data.user?.firstname + ' ' + data.user?.lastname);
                this.router.navigate(['home']);
              }
            })
          }else{ //Revisar porque no va a este 
            Swal.fire({ 
              icon: 'error',
              title: 'Error 😣',
              text: 'El correo o la contraseña son incorrectos',
              showConfirmButton: false,
              timer: 2000
            });
          }
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Ups ☹ !',
            text: 'No se pudo llevar a cabo la petición',
            showConfirmButton: false,
            timer: 2000
          }).then((res: any) => {
            if(res){
              throw new Error(err);
            }
          });
        }
      });
    }
  }


  // Este login es para la APIREST 
  loginApiRest(){

    let user: any = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };

   if(user){
    this.authSvc.loginApiRest(user)
    .pipe(first())
    .subscribe({
      next: (data) => {
        if(data){
          Swal.fire({
            icon: 'success',
            title: 'Logueo exitoso <span style="color: green; font-size:2em;">☺</span>',
            text: 'Los datos son correctos',
            showConfirmButton: false,
            timer: 2000
          }).then((res) => {
            if(res){
              this.storage.set("isLoggedIn", data.ok);
              this.authSvc.setToken(data.token);
              this.cookie.set('email',data.email);
              this.router.navigate(['dashboard']);
            }
          });
        }else{
          Swal.fire({
            icon: 'error',
            title: 'Error 😣',
            text: 'El correo o la contraseña son incorrectos',
            showConfirmButton: false,
            timer: 2000
          });
        }
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Ups ☹ !',
          text: 'Hubo un problema con el servidor',
          showConfirmButton: false,
          timer: 2000
        }).then((res: any) => {
          if(res){
            throw new Error(err);
          }
        });
      }
    });
   }
  }


  // Para mostrar el formulario y registrarse a FIS *******************************************************************/
  registerForm: any = new FormGroup({
    username: new FormControl('',Validators.required),
    email: new FormControl('',Validators.required),
    password: new FormControl('',Validators.required),
    confirmPassword: new FormControl('',Validators.required)
  });
  users: any[] = [];
  
  isDialogFIS(){
    if(this.checked){
      this.dialogFIS = true;
    }
  }

  isDialogResetFIS(){
    if(this.checked){
      this.page.setTitle("Cambiar contraseña");
      this.dialogEmailConfirmFIS = true;
    }
  }

  // Para registrar un nuevo usuario a la facultad
  RegisterFIS(){

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
        this.authSvc.getUsersJsonServer().subscribe((data: any) => {
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
            this.authSvc.postUserJsonServer(user).subscribe({
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

  // Para confirmar si existe tal usuario en la base de datos 
  EmailConfirm(){
    this.authSvc.getUsersJsonServer().subscribe({
      next: (data) => {
        for(let i of Object.values(data)){
          if(this.emailFormFIS.value.email !== i.email){
            this.dialogEmailConfirmFIS = false;
            Swal.fire({
              icon: 'error',
              text: 'El correo electrónico es incorrecto',
              showConfirmButton: false,
              timer: 1500
            }).then((res: any) => {
              if(res){
                this.dialogEmailConfirmFIS = true;
                this.emailFormFIS.reset();
              }
            });
            break;
          }else{
            this.dialogEmailConfirmFIS = false;
            Swal.fire({
              icon: 'success',
              text: 'El dato es correcto',
              showConfirmButton: false,
              timer: 1500
            }).then((result: any) => {
              if(result){
                this.dialogPassResetFIS = true;
                this.cookie.set('id',i.id);
              }
            });
            break;
          }
        }
      },
      error: (err) => {
        throw new Error("Error" + err);
      }
    })
  }

  ChangePassword(){
    if(this.resetFormFIS.value.password != this.resetFormFIS.value.confirmPassword){
      this.dialogPassResetFIS = false
      Swal.fire({
        icon: 'error',
        text: 'Las contraseñas no coinciden',
        showConfirmButton: false,
        timer: 2000
      }).then((res: any) => {
        if(res){
          this.dialogPassResetFIS = true;
          this.resetFormFIS.reset();
        }
      })
    }else{
      this.dialogPassResetFIS = false;
      this.authSvc.getUsersJsonServer().subscribe((data) => {
        let id = this.cookie.get('id');
        let user = Object.values(data);
        let res = user.filter(x => x.id == id);

        for(let i of res){

          let password: IUser = {
            id: i.id,
            firstname: i.firstname,
            lastname: i.lastname,
            email: i.email,
            password: this.resetFormFIS.value.password,
            confirmPassword: this.resetFormFIS.value.confirmPassword
          }

          this.authSvc.putUserJsonServer(password).subscribe({
            next: (data) => {
              if(data){
                Swal.fire({
                  icon: 'success',
                  text: 'La contraseña se cambió correctamente',
                  showConfirmButton: false,
                  timer: 1500
                }).then((res) => {
                  if(res){
                    this.cookie.delete('id');
                    this.page.setTitle("Iniciar sesión");
                    this.router.navigate(['/']);
                  }
                });
              }
            },
            error: (err) => {
              throw new Error("Error" + err);
            }
          });
          break;
        }
      });
    }
  }
}
