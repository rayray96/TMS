import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AppMaterialModule } from '../app-material/app-material.module';
import { ManagerRoutingModule } from './manager-routing.module';
import {
    ManageTeamComponent,
    ManagerHomeComponent,
    TeamMemberInfoComponent,
    TeamMembersListComponent,
    ManageTasksComponent,
    TaskInfoComponent,
    TaskListComponent,
    EditTaskComponent
} from '.';

@NgModule({
    imports: [
        CommonModule,
        AppMaterialModule,
        FormsModule,
        ManagerRoutingModule
    ],
    declarations: [
        ManageTeamComponent,
        ManagerHomeComponent,
        TeamMemberInfoComponent,
        TeamMembersListComponent,
        ManageTasksComponent,
        TaskInfoComponent,
        TaskListComponent,
        EditTaskComponent,
    ],
    entryComponents: [
        EditTaskComponent
    ],
    exports: [
        ManageTeamComponent,
        ManagerHomeComponent,
        TeamMemberInfoComponent,
        TeamMembersListComponent,
        ManageTasksComponent,
        TaskInfoComponent,
        TaskListComponent,
        EditTaskComponent
    ]
})
export class ManagerModule { }
