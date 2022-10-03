import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../app/shared/guards/authGuard.guard';

// Components 
import { LogInComponent } from './global/views/login/log-in/log-in.component';
import { PageNotFoundComponent } from './global/views/page-not-found/page-not-found.component';
import { LogUpComponent } from './global/views/login/log-up/log-up.component';
import { ResetPassComponent } from './global/views/login/reset-pass/reset-pass.component';
import { HomeComponent } from './global/views/home/home.component';
import { DashboardComponent } from './global/views/dashboard/dashboard.component';


const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full'},
  { path: 'login', component: LogInComponent},
  { path: 'logup', component: LogUpComponent},
  { path: 'reset-pass', component: ResetPassComponent},
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]},
  { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
