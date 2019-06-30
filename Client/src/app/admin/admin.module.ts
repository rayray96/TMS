import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AdminHomeComponent, ManageUsersComponent, RoleComponent, UserDetailComponent, UserDetailListComponent } from './index';
import { AppMaterialModule } from '../app-material/app-material.module';
import { AdminRoutingModule } from './admin-routing.module';
import { CommonComponentsModule } from 'src/app/common/common-components.module';

@NgModule({
    imports: [
        CommonModule,
        AppMaterialModule,
        FormsModule,
        AdminRoutingModule,
        CommonComponentsModule
    ],
    declarations: [
        AdminHomeComponent,
        ManageUsersComponent,
        RoleComponent,
        UserDetailComponent,
        UserDetailListComponent
    ],
    entryComponents: [
        RoleComponent
    ],
    exports: [
        AdminHomeComponent,
        ManageUsersComponent,
        RoleComponent,
        UserDetailComponent,
        UserDetailListComponent
    ]
})
export class AdminModule { }
