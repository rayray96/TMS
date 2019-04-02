import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './auth/auth.guard';
import { Error404Component } from './components/errors/error404/error404.component';
import { Error400Component } from './components/errors/error400/error400.component';
import { Error500Component } from './components/errors/error500/error500.component';
import { ErrorComponent } from './components/errors/error/error.component';
import { AdminGuard } from './auth/admin.guard';
import { ManagerGuard } from './auth/manager.guard';

const routes: Routes = [
  { path: '', redirectTo: '/user/login', pathMatch: 'full' },
  {
    path: 'user', component: UserComponent,
    children: [
      { path: 'registration', component: RegistrationComponent },
      { path: 'login', component: LoginComponent }
    ]
  },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: '**', component: Error404Component }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthGuard, AdminGuard, ManagerGuard] // Added.
})
export class AppRoutingModule { }
