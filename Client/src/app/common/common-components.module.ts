import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppMaterialModule } from '../app-material/app-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import {
  DropdownlistComponent,
  ContinuousScrollComponent,
  SpinnerLoaderComponent,
  SortComponent,
  SearchPipe,
  EnumToArrayPipe,
  SearchComponent,
  MultipleSelectComponent,
  OrderByPipe
} from './index';

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
    MultipleSelectComponent,
    OrderByPipe
  ],
  imports: [
    CommonModule,
    AppMaterialModule,
    FormsModule,
    NgbModule,
    ReactiveFormsModule
  ],
  exports: [
    DropdownlistComponent,
    SpinnerLoaderComponent,
    SortComponent,
    ContinuousScrollComponent,
    SearchComponent,
    SearchPipe,
    EnumToArrayPipe,
    MultipleSelectComponent,
    OrderByPipe
  ]
})
export class CommonComponentsModule { }
