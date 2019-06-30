import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';

import { AppMaterialModule } from './app-material/app-material.module';
import { AppRoutingModule } from './app-routing.module';
import { AdminModule } from './admin/admin.module';
import { CommonComponentsModule } from './common/common-components.module';
import { AdminRoutingModule } from './admin/admin-routing.module';
import { ComponentsModule } from './components/components.module';
import { ComponentsRoutingModule } from './components/components-routing.module';
import { ManagerRoutingModule } from './manager/manager-routing.module';
import { ManagerModule } from './manager/manager.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NavBarService, UserService, JwtService, GlobalErrorHandler, TaskService } from './services';
import { WorkerModule } from './worker/worker.module';
import { WorkerRoutingModule } from './worker/worker-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    NavBarComponent
  ],
  imports: [
    AdminModule,
    AdminRoutingModule,
    CommonComponentsModule,
    ManagerModule,
    ManagerRoutingModule,
    WorkerModule,
    WorkerRoutingModule,
    ComponentsModule,
    ComponentsRoutingModule,
    AppMaterialModule,
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      progressBar: true
    }),
    NgbModule,
    // Routing module should always be in the end of imports!
    AppRoutingModule
  ],
  providers: [
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    JwtService,
    UserService,
    NavBarService,
    TaskService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
