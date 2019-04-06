import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppMaterialModule } from '../app-material/app-material.module';
import { FormsModule } from '@angular/forms';

import { WorkerRoutingModule } from './worker-routing.module';
import {
  WorkerHomeComponent,
  MyTasksComponent,
  TaskListComponent,
  TaskInfoComponent,
  EditStatusComponent
} from '.';

@NgModule({
  imports: [
    CommonModule,
    AppMaterialModule,
    FormsModule,
    WorkerRoutingModule
  ],
  declarations: [
    WorkerHomeComponent,
    MyTasksComponent,
    TaskListComponent,
    TaskInfoComponent,
    EditStatusComponent
  ],
  entryComponents: [
    EditStatusComponent
  ],
  exports: [
    WorkerHomeComponent,
    MyTasksComponent,
    TaskListComponent,
    TaskInfoComponent,
    EditStatusComponent
  ]
})
export class WorkerModule { }
