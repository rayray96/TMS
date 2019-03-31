import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/services';
import { RoleModel } from 'src/app/models';
import { MatDialog } from '@angular/material';
import { RoleComponent } from '../role/role.component';
import { filter, mergeMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  constructor(private admin: AdminService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {

  }

  public onEditRole(role: RoleModel): void {
    const dialogRef = this.dialog.open(RoleComponent, {
      height: '200px',
      width: '300px',
      data: { role }
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(response => !!response),
        mergeMap(response =>
          this.admin
            .updateRole((response as RoleModel), this.admin.currentUser.id)
            .pipe(mergeMap(_ => of(response)))
        )
      )
      .subscribe(
        success => {

          this.admin.currentUser.role = (success as RoleModel).role;
          this.spinner.hide();
          this.toastr.success("Role has changed!");
        },
        error => {
          console.log(error);
          this.spinner.hide();
          this.toastr.warning(error.errors);
        }
      );
  }
}
