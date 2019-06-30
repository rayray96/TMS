import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppMaterialModule } from '../app-material/app-material.module';
import { FormsModule } from '@angular/forms';

import {
  DropdownlistComponent,
  ContinuousScrollComponent,
  SpinnerLoaderComponent,
  SortComponent,
  SearchPipe,
  EnumToArrayPipe,
  SearchComponent
} from './index';
import { FilterPipe } from './filter/filter.pipe';

@NgModule({
  declarations: [
    DropdownlistComponent,
    SpinnerLoaderComponent,
    SortComponent,
    ContinuousScrollComponent,
    SearchPipe,
    SearchComponent,
    SearchComponent,
    EnumToArrayPipe,
    FilterPipe,
  ],
  imports: [
    CommonModule,
    AppMaterialModule,
    FormsModule,
    NgbModule
  ],
  exports: [
    DropdownlistComponent,
    SpinnerLoaderComponent,
    SortComponent,
    ContinuousScrollComponent,
    SearchComponent,
    SearchPipe,
    EnumToArrayPipe,
    FilterPipe
  ]
})
export class CommonComponentsModule { }
