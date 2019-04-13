import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { UserModel, TaskModel } from '../models';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavBarService } from '../services/nav-bar.service';
import { MatTableDataSource, MatSortable, MatSort, MatPaginator } from '@angular/material';
import { TaskService } from '../services';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['name', 'author', 'priority', 'assignee','progress', 'deadline'];
  userDetails: UserModel;
  dataSource;

  constructor(private router: Router,
    private task: TaskService,
    private service: UserService,
    private spinner: NgxSpinnerService,
    public nav: NavBarService) { }

  @ViewChild(MatSort)
  sort: MatSort;

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  ngOnInit() {
    this.getAllTasks();
    this.getUserProfile();
  }

  onSelect(taskModel: TaskModel): void {
    this.task.currentTask = taskModel;
  }

  ngOnDestroy(): void {
    this.task.currentTask = undefined;
  }

  getAllTasks() {
    this.spinner.show();
    this.service.getAllTasks().subscribe(
      res => {
        this.dataSource = new MatTableDataSource(res as TaskModel[]);

        this.sort.sort(<MatSortable>({ id: 'dateStart', start: 'asc' }));
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;

        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        throw (err);
      }
    );
  }

  getUserProfile() {
    this.nav.show();
    this.spinner.show();
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = (res as UserModel);
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        throw (err);
      },
    );
  }
}
