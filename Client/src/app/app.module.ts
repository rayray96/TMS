import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';

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
import { ManagerHomeComponent } from './manager/manager-home/manager-home.component';
import { ManageTeamComponent } from './manager/manage-team/manage-team.component';
import { TeamMembersListComponent } from './manager/team-members-list/team-members-list.component';
import { TeamMemberInfoComponent } from './manager/team-member-info/team-member-info.component';
import { PotentialMembersComponent } from './manager/potential-members/potential-members.component';
import { FiredMemberComponent } from './manager/fired-member/fired-member.component';
import { ManagerRoutingModule } from './manager/manager-routing.module';
import { AdminModule } from './admin/admin.module';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NavBarService } from './services/nav-bar.service';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    Error404Component,
    Error400Component,
    Error500Component,
    ErrorComponent,
    NavBarComponent,
    ManagerHomeComponent,
    ManageTeamComponent,
    TeamMembersListComponent,
    TeamMemberInfoComponent,
    PotentialMembersComponent,
    FiredMemberComponent
  ],
  imports: [
    AdminModule,
    AdminRoutingModule,
    AppMaterialModule,
    BrowserModule,
    ManagerRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      progressBar: true
    }),
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
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
