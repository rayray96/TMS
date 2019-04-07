import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Error404Component, Error400Component, Error500Component, ErrorComponent } from '.';
import { AuthGuard } from '../auth/auth.guard';
import { AboutComponent } from './about/about.component';
import { ContactMeComponent } from './contact-me/contact-me.component';

@NgModule({
  imports: [RouterModule.forChild([
    { path: 'about', component: AboutComponent, canActivate: [AuthGuard] },
    { path: 'contact-me', component: ContactMeComponent, canActivate: [AuthGuard] },
    { path: 'error404', component: Error404Component },
    { path: 'error400', component: Error400Component },
    { path: 'error500', component: Error500Component },
    { path: 'error', component: ErrorComponent },
  ])],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class ComponentsRoutingModule { }
