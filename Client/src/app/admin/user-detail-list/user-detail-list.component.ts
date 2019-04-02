import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator, MatSortable } from '@angular/material';
import { AdminService } from 'src/app/services';
import { NgxSpinnerService } from 'ngx-spinner';
import { UserModel } from 'src/app/models';

@Component({
  selector: 'app-user-detail-list',
  templateUrl: './user-detail-list.component.html',
  styleUrls: ['./user-detail-list.component.css']
})
export class UserDetailListComponent implements OnInit {
  displayedColumns: string[] = ['fName', 'lName', 'role', 'team'];
  dataSource;

  constructor(private admin: AdminService, private spinner: NgxSpinnerService) { }

  @ViewChild(MatSort)
  sort: MatSort;

  @ViewChild(MatPaginator)
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
        console.log(err);
        this.spinner.hide();
      },
    );
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onSelect(userModel: UserModel): void {
    this.admin.currentUser = userModel;
  }
}
