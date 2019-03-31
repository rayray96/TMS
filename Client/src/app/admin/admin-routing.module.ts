import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ManageUsersComponent } from './manage-users/manage-users.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AuthGuard } from '../auth/auth.guard';
import { AdminGuard } from '../auth/admin.guard';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'admin',
        component: AdminHomeComponent,
        canActivate: [AuthGuard, AdminGuard],
        children: [
          {
            path: '',
            children: [
              { path: 'users', component: ManageUsersComponent },
              { path: '', redirectTo: 'users', pathMatch: 'full' }
            ]
          }
        ]
      }
    ])],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
