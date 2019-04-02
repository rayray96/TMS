import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComponentsRoutingModule } from './components-routing.module';
import { ErrorComponent, Error400Component, Error404Component, Error500Component } from '.';

@NgModule({
  declarations: [
    ErrorComponent,
    Error400Component,
    Error404Component,
    Error500Component
  ],
  exports: [
    ErrorComponent,
    Error400Component,
    Error404Component,
    Error500Component,
  ],
  imports: [
    CommonModule,
    ComponentsRoutingModule
  ]
})
export class ComponentsModule { }
