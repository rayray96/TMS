import { Component, OnInit, Inject } from '@angular/core';
import { Role, RoleModel } from 'src/app/models';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AdminService } from 'src/app/services';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.css']
})
export class RoleComponent {
  Role: Role;
  onSelectUserRole: string;

  constructor(
    public dialogRef: MatDialogRef<RoleComponent>,
    private admin: AdminService,
    private spinner: NgxSpinnerService,
    @Inject(MAT_DIALOG_DATA) 
    public role: RoleModel
    ) { 
      role.role = this.admin.currentUser.role;
    }
    

  public onNoClick(): void {
    this.dialogRef.close(this.role);
  }

  public onSaveCLick(): void {
    this.spinner.show();
    this.dialogRef.close(this.role);
  }

  public onCloseCLick(): void {
    this.dialogRef.close();
  }
}
