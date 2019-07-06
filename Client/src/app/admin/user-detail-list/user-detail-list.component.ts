import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatSortable } from '@angular/material';
import { AdminService } from 'src/app/services';
import { NgxSpinnerService } from 'ngx-spinner';
import { UserModel } from 'src/app/models';
import { throwError } from 'rxjs';
import { Roles } from 'src/app/common';

@Component({
  selector: 'app-user-detail-list',
  templateUrl: './user-detail-list.component.html',
  styleUrls: ['./user-detail-list.component.css']
})
export class UserDetailListComponent implements OnInit {
  displayedColumns: string[] = ['fName', 'lName', 'role', 'team'];
  roles = Roles;
  dataSource;
  currentSelection = 'All';
  
  columns: string[] = ['First Name', 'Last Name', 'Role', 'Team'];
  currentSelections;

  getCurrentSelections() {
    if (this.currentSelections !== null && this.currentSelections !== undefined) {
      var newSelections: string[];
      this.currentSelections.forEach(element => {
        switch (element) {
          case this.columns[0]: {
            newSelections.push(this.displayedColumns[0]);
            break;
          }
          case this.columns[1]: {
            newSelections.push(this.displayedColumns[1]);
            break;
          }
          case this.columns[2]: {
            newSelections.push(this.displayedColumns[2]);
            break;
          }
          case this.columns[3]: {
            newSelections.push(this.displayedColumns[3]);
            break;
          }
          default: {
            break;
          }
        }
      });
      return newSelections.values;
    }
  }

  constructor(private admin: AdminService, private spinner: NgxSpinnerService) { }

  @ViewChild(MatSort, { static: true })
  sort: MatSort;

  @ViewChild(MatPaginator, { static: true })
  paginator: MatPaginator;

  ngOnInit() {
    this.spinner.show();
    this.admin.getAll().subscribe(
      res => {
        this.dataSource = new MatTableDataSource(res as UserModel[]);
        this.sort.sort(<MatSortable>({ id: 'fName', start: 'asc' }));
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        throw (err);
      },
    );
  }
  // Unusing anymore.
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onSelect(userModel: UserModel): void {
    this.admin.currentUser = userModel;
  }
}
