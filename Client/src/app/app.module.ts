import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserService } from './services/user.service';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { JwtService } from './services/jwt.service';
import { GlobalErrorHandler } from './services/global-error-handler.service';
import { Error404Component } from './components/errors/error404/error404.component';
import { Error400Component } from './components/errors/error400/error400.component';
import { Error500Component } from './components/errors/error500/error500.component';
import { ErrorComponent } from './components/errors/error/error.component';
import { AppMaterialModule } from './app-material/app-material.module';
import { AdminRoutingModule } from './admin/admin-routing.module';
import { AdminHomeComponent } from './admin/admin-home/admin-home.component';
import { ManageUsersComponent } from './admin/manage-users/manage-users.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminHomeComponent,
    ManageUsersComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    Error404Component,
    Error400Component,
    Error500Component,
    ErrorComponent,
    NavBarComponent
  ],
  imports: [
    AppMaterialModule,
    BrowserModule,
    AdminRoutingModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      progressBar: true
    })
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    JwtService,
    UserService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
