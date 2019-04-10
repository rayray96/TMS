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
        this.toastr.warning(res.message);
      },
      (err: any) => {
        this.spinner.hide();
        this.toastr.error(err.error);
      }
    );
  }

  finishTask() {
    this.spinner.show();
    const status = 'Completed';
    this.task.updateStatus(this.managerId, { taskId: this.task.currentTask.id, status: status }).subscribe(
      success => {
        this.task.needCheck = true;
        this.task.currentTask.status = status;
        this.spinner.hide();
        this.toastr.success("Task has finished!");
      },
      error => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Cannot finish a task');
      }
    );
  }

  cancelTask() {
    this.spinner.show();
    const status = 'Canceled';
    this.task.updateStatus(this.managerId, { taskId: this.task.currentTask.id, status: status }).subscribe(
      success => {
        this.task.needCheck = true;
        this.task.currentTask.status = status;
        this.spinner.hide();
        this.toastr.warning("Task has canceled!");
      },
      error => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Cannot cancel a task');
      }
    );
  }

  resumeTask() {
    this.spinner.show();
    const status = 'Not Started';
    this.task.updateStatus(this.managerId, { taskId: this.task.currentTask.id, status: status }).subscribe(
      success => {
        this.task.needCheck = true;
        this.task.currentTask.status = status;
        this.spinner.hide();
        this.toastr.success("Task has resumed!");
      },
      error => {
        console.log(error);
        this.spinner.hide();
        this.toastr.error('Cannot resume a task');
      }
    );
  }
}
