import { Component, OnInit, OnDestroy } from '@angular/core';
import { ManagerService, TaskService, JwtService } from 'src/app/services';
import { MatDialog } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { EditTaskComponent } from '../edit-task/edit-task.component';
import { filter, mergeMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { CreateTaskModel, TaskModel, StatusModel, EditStatusModel } from 'src/app/models';

@Component({
  selector: 'app-task-info',
  templateUrl: './task-info.component.html',
  styleUrls: ['./task-info.component.css']
})
export class TaskInfoComponent implements OnInit, OnDestroy {
  taskForChange: TaskModel;
  statusForChange: EditStatusModel;
  managerId: string;

  constructor(private manager: ManagerService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private task: TaskService,
    private jwt: JwtService) {
    this.managerId = this.jwt.getId();
  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    this.task.currentTask = undefined;
  }

  updateTask(): void {
    const taskForChange = Object.assign({}, this.task.currentTask);

    const dialogRef = this.dialog.open(EditTaskComponent, {
      height: '450px',
      width: '300px',
      data: taskForChange
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(response => !!response),
        mergeMap(response =>
          this.task
            .updateTask((response as TaskModel).id, (response as CreateTaskModel))
            .pipe(mergeMap(_ => of(response)))
        )
      )
      .subscribe(
        success => {
          this.task.currentTask = success as TaskModel;
          this.task.needCheck = true;
          this.spinner.hide();
          this.toastr.success("Task has updated!");
        },
        error => {
          console.log(error);
          this.spinner.hide();
          this.toastr.warning('Cannot update a task');
        }
      );
  }

  deleteTask() {
    this.spinner.show();
    this.task.deleteTask(this.task.currentTask.id).subscribe(
      (res: any) => {
        this.task.needCheck = true;
        this.task.currentTask = undefined;
        this.spinner.hide();
        this.toastr.success(res.message);
      },
      (err: any) => {
        this.spinner.hide();
        this.toastr.success(err.error);
      }
    );
  }

finishTask(){
this.spinner.show();
this.statusForChange.id = this.task.currentTask.id;
this.statusForChange.status = 'Completed';
this.task.updateStatus(this.managerId, this.statusForChange ).subscribe(
  success => {
    this.task.currentTask.status = this.statusForChange.status;
    this.task.needCheck = true;
    this.spinner.hide();
    this.toastr.warning("Status has finished!");
  },
  error => {
    console.log(error);
    this.spinner.hide();
    this.toastr.error('Cannot finish a task');
  }
);
}

cancelTask(){
  this.spinner.show();
  this.statusForChange.id = this.task.currentTask.id;
  this.statusForChange.status = 'Canceled';
  this.task.updateStatus(this.managerId, this.statusForChange ).subscribe(
    success => {
      this.task.currentTask.status = this.statusForChange.status;
      this.task.needCheck = true;
      this.spinner.hide();
      this.toastr.warning("Status has canceled!");
    },
    error => {
      console.log(error);
      this.spinner.hide();
      this.toastr.error('Cannot cancel a task');
    }
  );
}

  setStatus(): void {
    const statusForChange = Object.assign({}, this.task.currentTask as StatusModel);

    const dialogRef = this.dialog.open(EditTaskComponent, {
      height: '200px',
      width: '300px',
      data: statusForChange
    });

    dialogRef
      .afterClosed()
      .pipe(
        filter(response => !!response),
        mergeMap(response =>
          this.task
            .updateStatus(this.managerId, (response as EditStatusModel))
            .pipe(mergeMap(_ => of(response)))
        )
      )
      .subscribe(
        success => {
          this.task.currentTask.status = (success as StatusModel).name;
          this.task.needCheck = true;
          this.spinner.hide();
          this.toastr.success("Status has updated!");
        },
        error => {
          console.log(error);
          this.spinner.hide();
          this.toastr.error('Cannot update a status');
        }
      );
  }
}
