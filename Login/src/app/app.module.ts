import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Routes 
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

// Forms 
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Librearies
import { PrimengModule } from './global/libraries/primeng.module';

// Session 
import { LogInComponent } from './global/views/login/log-in/log-in.component';
import { LogUpComponent } from './global/views/login/log-up/log-up.component';
import { ResetPassComponent } from './global/views/login/reset-pass/reset-pass.component';

// AuhtenticateService
import { AuthGuard } from './shared/guards/authGuard.guard';
import { AutehnticateService } from './shared/auth/autehnticate.service';
import { AuthInterceptor } from './shared/interceptor/AuthInterceptor.interceptor';

// Layouts 
import { NavbarComponent } from './global/layouts/navbar/navbar.component';
import { FooterComponent } from './global/layouts/footer/footer.component';

// Pipes
import { ReversedPipe } from './shared/pipes/reversed.pipe';

// Web Application 
import { HomeModule } from './global/views/home/home.module';
import { DashboardModule } from './global/views/dashboard/dashboard.module';

// App component and page not found
import { AppComponent } from './app.component';
import { PageNotFoundComponent } from './global/views/page-not-found/page-not-found.component';


@NgModule({
  declarations: [
    AppComponent,
    LogInComponent,
    LogUpComponent,
    ResetPassComponent,
    PageNotFoundComponent,
    ReversedPipe,
    NavbarComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    PrimengModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HomeModule,
    DashboardModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    AutehnticateService,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
