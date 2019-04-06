import { Component, OnInit, Inject } from '@angular/core';
import { StatusModel } from 'src/app/models';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NgxSpinnerService } from 'ngx-spinner';
import { TaskService } from 'src/app/services';

@Component({
  selector: 'app-edit-status',
  templateUrl: './edit-status.component.html',
  styleUrls: ['./edit-status.component.css']
})
export class EditStatusComponent implements OnInit {
  statuses: StatusModel[];

  constructor(
    public dialogRef: MatDialogRef<EditStatusComponent>,
    private spinner: NgxSpinnerService,
    private task: TaskService,
    @Inject(MAT_DIALOG_DATA) public status: StatusModel
  ) {
    this.getStatuses();
  }

  ngOnInit(): void {
  }

  public onNoClick(): void {
    this.dialogRef.close(this.task);
  }

  public onSaveCLick(): void {
    this.spinner.show();
    this.dialogRef.close(this.task);
  }

  public onCloseCLick(): void {
    this.dialogRef.close();
  }

  private getStatuses() {
    this.spinner.show();
    this.task.getStatuses().subscribe(
      res => {
        this.statuses = res as StatusModel[];
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
      }
    );
  }
}
