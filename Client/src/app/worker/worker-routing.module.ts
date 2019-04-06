import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { WorkerHomeComponent, MyTasksComponent } from '.';
import { AuthGuard } from '../auth/auth.guard';
import { WorkerGuard } from '../auth/worker.guard';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: 'worker',
        component: WorkerHomeComponent,
        canActivate: [AuthGuard, WorkerGuard],
        children: [
          {
            path: '',
            children: [
              { path: 'tasks', component: MyTasksComponent },
              { path: '', redirectTo: 'tasks', pathMatch: 'full' }
            ]
          }
        ]
      }
    ])],
  exports: [RouterModule]
})
export class WorkerRoutingModule { }
