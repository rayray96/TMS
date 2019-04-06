import { Component, OnInit, ViewChild, DoCheck } from '@angular/core';
import { ManagerService, TaskService, JwtService } from 'src/app/services';
import { ToastrService } from 'ngx-toastr';
import { MatDialog, MatSortable, MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { NgxSpinnerService } from 'ngx-spinner';
import { filter, mergeMap } from 'rxjs/operators';
import { CreateTaskModel, TaskModel } from 'src/app/models';
import { EditTaskComponent } from '../edit-task/edit-task.component';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit, DoCheck {
  displayedColumns: string[] = ['name', 'priority', 'progress', 'deadline'];
  managerId;
  dataSource;

  constructor(private manager: ManagerService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private task: TaskService,
    private jwt: JwtService) {
    this.managerId = this.jwt.getId();
  }

  @ViewChild(MatSort)
  sort: MatSort;

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  ngOnInit() {
    this.getTasksOfMyTeam();
    console.log('ngOnInit');
  }

  ngDoCheck() {
    if (this.task.needCheck) {
      this.getTasksOfMyTeam();
      console.log('ngDoCheck');
    }
  }

  createTask(): void {
    const dialogRef = this.dialog.open(EditTaskComponent, {
      height: '450px',
      width: '300px',
      data: {}
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(response => !!response),
        mergeMap(response =>
          this.task.createTask(response as CreateTaskModel)
        )
      )
      .subscribe(
        success => {
          this.task.needCheck = true;
          this.spinner.hide();
          this.toastr.success("Task has created!");
        },
        error => {
          console.log(error);
          this.spinner.hide();
          this.toastr.warning('Cannot create a task');
        }
      );
  }

  onSelect(taskModel: TaskModel): void {
    this.task.currentTask = taskModel;
  }

  private getTasksOfMyTeam() {
    this.spinner.show();
    this.task.needCheck = false;
    this.task.getTasksOfManager(this.managerId).subscribe(
      res => {
        this.dataSource = new MatTableDataSource(res as TaskModel[]);

        this.sort.sort(<MatSortable>({ id: 'name', start: 'asc' }));
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;

        if (this.task.currentTask) {
          this.task.currentTask = this.dataSource.data.filter(x => x.id === this.task.currentTask.id)[0];
        }

        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        console.log(err);
      }
    );
  }
}
