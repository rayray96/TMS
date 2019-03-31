import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ManagerHomeComponent } from './manager-home/manager-home.component';
import { AuthGuard } from '../auth/auth.guard';
import { ManagerGuard } from '../auth/manager.guard';
import { ManageTeamComponent } from './manage-team/manage-team.component';

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
              { path: '', redirectTo: 'team', pathMatch: 'full' }
            ]
          }
        ]
      }
    ])],
  exports: [RouterModule]
})
export class ManagerRoutingModule { }
