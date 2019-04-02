import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Error404Component, Error400Component, Error500Component, ErrorComponent } from '.';

@NgModule({
  imports: [RouterModule.forChild([
    { path: 'error404', component: Error404Component },
    { path: 'error400', component: Error400Component },
    { path: 'error500', component: Error500Component },
    { path: 'error', component: ErrorComponent },
  ])],
  exports: [RouterModule]
})
export class ComponentsRoutingModule { }
