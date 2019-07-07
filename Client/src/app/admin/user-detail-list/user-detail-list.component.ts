import { Component, OnInit, ViewChild, OnChanges, ChangeDetectorRef } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatSortable } from '@angular/material';
import { AdminService } from 'src/app/services';
import { NgxSpinnerService } from 'ngx-spinner';
import { UserModel } from 'src/app/models';
import { Roles, UserManagementEnum, SortingsEnum } from 'src/app/common';

@Component({
  selector: 'app-user-detail-list',
  templateUrl: './user-detail-list.component.html',
  styleUrls: ['./user-detail-list.component.css']
})
export class UserDetailListComponent implements OnInit, OnChanges {

  displayedColumns: string[] = ['fName', 'lName', 'role', 'team'];
  dataSource;

  currentSelection = 'All';
  currentSortKey = 'fName';
  currentSortValue = 'First Name';
  currentSortKeyBy = 'asc';
  currentSortValueBy = 'Ascending';

  roles = Roles;
  columns = UserManagementEnum;
  sortings = SortingsEnum;


  constructor(private admin: AdminService, private spinner: NgxSpinnerService, private cdr: ChangeDetectorRef) {
  }

  ngOnChanges() {
  }

  sortValues(column: string, sortType: string) {
    console.log(column);
    console.log(sortType);
    this.currentSortKey = Object.keys(UserManagementEnum).filter(x => UserManagementEnum[x] == column)[0];
    this.currentSortKeyBy = Object.keys(SortingsEnum).filter(x => SortingsEnum[x] == sortType)[0];
    this.cdr.markForCheck();
  }

  @ViewChild(MatSort, { static: true })
  sort: MatSort;

  @ViewChild(MatPaginator, { static: true })
  paginator: MatPaginator;

  ngOnInit() {
    this.spinner.show();

    this.admin.getAll().subscribe(
      res => {
        this.dataSource = new MatTableDataSource(res as UserModel[]);
        //this.sort.sort(<MatSortable>({ id: 'fName', start: 'asc' }));
        this.dataSource.paginator = this.paginator;
        //this.dataSource.sort = this.sort;
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        throw (err);
      },
    );
  }

  // Unusing anymore.
  // applyFilter(filterValue: string) {
  //   this.dataSource.filter = filterValue.trim().toLowerCase();
  // }

  onSelect(userModel: UserModel): void {
    this.admin.currentUser = userModel;
  }
}
