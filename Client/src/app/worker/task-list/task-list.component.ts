import { Component, OnInit, ViewChild, DoCheck } from '@angular/core';
import { MatDialog, MatSort, MatPaginator, MatTableDataSource, MatSortable } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { TaskService, JwtService } from 'src/app/services';
import { TaskModel } from 'src/app/models';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit, DoCheck {
  displayedColumns: string[] = ['name', 'priority', 'progress', 'deadline'];
  workerId;
  dataSource;

  constructor(
    private spinner: NgxSpinnerService,
    private task: TaskService,
    private jwt: JwtService) {
    this.workerId = this.jwt.getId();
  }

  @ViewChild(MatSort)
  sort: MatSort;

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  ngOnInit() {
    this.getMyTasks();
    console.log('ngOnInit');
  }

  ngDoCheck() {
    if (this.task.needCheck) {
      this.getMyTasks();
      console.log('ngDoCheck');
    }
  }

  private getMyTasks() {
    this.spinner.show();
    this.task.needCheck = false;
    this.task.getTasksOfWorker(this.workerId).subscribe(
      res => {
        this.dataSource = new MatTableDataSource(res as TaskModel[]);

        this.sort.sort(<MatSortable>({ id: 'name', start: 'asc' }));
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;

        if(this.task.currentTask){
          this.task.currentTask = this.dataSource.data.filter(x=>x.id === this.task.currentTask.id)[0];
          console.log(this.task.currentTask);
        }
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        throw (err);
      }
    );

  }

  onSelect(taskModel: TaskModel): void {
    this.task.currentTask = taskModel;
  }
}
