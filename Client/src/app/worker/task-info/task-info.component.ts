import { Component, OnInit } from '@angular/core';
import { TaskModel, StatusModel, EditStatusModel } from 'src/app/models';
import { MatDialog } from '@angular/material';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { TaskService, JwtService } from 'src/app/services';
import { EditStatusComponent } from '../edit-status/edit-status.component';
import { filter, mergeMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-task-info',
  templateUrl: './task-info.component.html',
  styleUrls: ['./task-info.component.css']
})
export class TaskInfoComponent implements OnInit {

  taskForChange: TaskModel;
  workerId: string;

  constructor(
    private dialog: MatDialog,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private task: TaskService,
    private jwt: JwtService) {
    this.workerId = this.jwt.getId();
  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    this.task.currentTask = undefined;
  }

    setStatus(): void {
    const statusForChange = Object.assign({}, this.task.currentTask as StatusModel);

    const dialogRef = this.dialog.open(EditStatusComponent, {
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
            .updateStatus(this.workerId, (response as EditStatusModel))
            .pipe(mergeMap(_ => of(response)))
        )
      )
      .subscribe(
        success => {
          this.task.needCheck = true;
          this.task.currentTask.status = (success as StatusModel).name;
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
