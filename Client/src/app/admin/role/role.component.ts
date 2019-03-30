import { Component, OnInit, Inject } from '@angular/core';
import { Role, RoleModel } from 'src/app/models';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AdminService } from 'src/app/services';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.css']
})
export class RoleComponent {
  Role: Role;

  constructor(
    public dialogRef: MatDialogRef<RoleComponent>,
    private admin: AdminService,
    @Inject(MAT_DIALOG_DATA) public role: RoleModel
  ) { }

  public onNoClick(): void {
    this.dialogRef.close(this.role);
  }

  public onSaveCLick(): void {
    this.dialogRef.close(this.role);
  }

  public onCloseCLick(): void {
    this.dialogRef.close();
  }
}
