import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ManagerHomeComponent, ManageTeamComponent, ManageTasksComponent } from '.';
import { AuthGuard } from '../auth/auth.guard';
import { ManagerGuard } from '../auth/manager.guard';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'manager',
        component: ManagerHomeComponent,
        canActivate: [AuthGuard, ManagerGuard],
        children: [
          {
            path: '',
            children: [
              { path: 'team', component: ManageTeamComponent },
              { path: 'tasks', component: ManageTasksComponent },
              { path: '', redirectTo: 'team', pathMatch: 'full' }
            ]
          }
        ]
      }
    ])],
  exports: [RouterModule]
})
export class ManagerRoutingModule { }
