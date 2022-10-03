import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { AutehnticateService } from '../../../../shared/auth/autehnticate.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CookieService } from 'ngx-cookie-service';
import { IUserRest } from '../../../../shared/models/interfaces/user.interface';

@Component({
  selector: 'app-reset-pass',
  templateUrl: './reset-pass.component.html',
  styleUrls: ['./reset-pass.component.scss']
})
export class ResetPassComponent implements OnInit {

  constructor(private cookie: CookieService,
              private page: Title,
              private authSvc: AutehnticateService,
              private router: Router) 
  { }

  formPass: boolean = false;
  emailForm: any = new FormGroup({
    email: new FormControl('',Validators.required)
  })
  resetForm: any = new FormGroup({
    password: new FormControl('',Validators.required),
    confirmPassword: new FormControl('',Validators.required)
  })

  ngOnInit(): void {
    this.page.setTitle("Reestablecer contraseña");
    // this.cookie.delete('id');
    // this.authSvc.getUsersApiRest().subscribe(data => {
    //   console.log(data);
    //   let id = this.cookie.get('id');
    //   let user = Object.values(data);
    //   let res = user.filter(x => x.userId == id);
    //   console.log(res)
    // })
  }


  //Methods
  EmailConfirm(){
    this.authSvc.getUsersApiRest().subscribe({
      next: (data) => {
        for(let i of Object.values(data)){
          if(this.emailForm.value.email !== i.email){
            Swal.fire({
              icon: 'error',
              text: 'El correo electrónico es incorrecto',
              showConfirmButton: false,
              timer: 1500
            });
            break;
          }else{
            Swal.fire({
              icon: 'success',
              text: 'El dato es correcto',
              showConfirmButton: false,
              timer: 1500
            }).then((result: any) => {
              if(result){
                this.formPass = true;
                this.cookie.set('id',i.userId);
              }
            });
            break;
          }
        }
      },
      error: (err) => {
        throw new Error("Error" + err);
      }
    });

  }

  ChangePassword(){
    if(this.resetForm.value.password != this.resetForm.value.confirmPassword){
      Swal.fire({
        icon: 'error',
        text: 'Las contraseñas no coinciden',
        showConfirmButton: false,
        timer: 2000
      }).then((res: any) => {
        if(res){
          this.resetForm.reset();
        }
      })
    }else{
      this.authSvc.getUsersApiRest().subscribe((data) => {
        let id = this.cookie.get('id');
        let user = Object.values(data);
        let res = user.filter(x => x.userId == id);

        for(let i of res){

          let password: IUserRest = {
            userId: i.userId,
            userName: i.userName,
            email: i.email,
            password: this.resetForm.value.password,
            confirmPassword: this.resetForm.value.confirmPassword
          }

          this.authSvc.putPassApiRest(password).subscribe({
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
