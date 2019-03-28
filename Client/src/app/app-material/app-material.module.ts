import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  MatButtonModule,
  MatCardModule,
  MatDialogModule,
  MatInputModule,
  MatTableModule,
  MatToolbarModule,
  MatMenuModule,
  MatIconModule,
  MatProgressSpinnerModule,
  MatGridListModule,
  MatDividerModule,
  MatExpansionModule,
  MatDatepickerModule,
  MatOptionModule,
  MatSelectModule,
  MatNativeDateModule,
  MatSidenavModule,
  MatListModule
} from '@angular/material';

import { DragDropModule } from '@angular/cdk/drag-drop';
import { LayoutModule } from '@angular/cdk/layout';

@NgModule({
  imports: [
    CommonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    LayoutModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatDialogModule,
    MatTableModule,
    MatMenuModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    MatDividerModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatOptionModule,
    MatSelectModule,
    MatNativeDateModule,
    DragDropModule
  ],
  exports: [
    CommonModule,
    LayoutModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatDialogModule,
    MatTableModule,
    MatMenuModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    MatDividerModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatOptionModule,
    MatSelectModule,
    MatNativeDateModule,
    DragDropModule
  ]
})
export class AppMaterialModule { }
