import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComponentsRoutingModule } from './components-routing.module';
import { ErrorComponent, Error400Component, Error404Component, Error500Component } from '.';
import { AboutComponent } from './about/about.component';
import { ContactMeComponent } from './contact-me/contact-me.component';

@NgModule({
  declarations: [
    ErrorComponent,
    Error400Component,
    Error404Component,
    Error500Component,
    AboutComponent,
    ContactMeComponent
  ],
  exports: [
    ErrorComponent,
    Error400Component,
    Error404Component,
    Error500Component,
    AboutComponent,
    ContactMeComponent
  ],
  imports: [
    CommonModule,
    ComponentsRoutingModule
  ]
})
export class ComponentsModule { }
