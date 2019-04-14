import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatOptionSelectionChange } from '@angular/material';
import { PersonModel, TeamModel, TaskModel } from 'src/app/models';
import { NgxSpinnerService } from 'ngx-spinner';
import { ManagerService, JwtService } from 'src/app/services';

@Component({
  selector: 'app-edit-task',
  templateUrl: './edit-task.component.html',
  styleUrls: ['./edit-task.component.css']
})
export class EditTaskComponent {
  myTeam: PersonModel[];
  managerId;

  constructor(
    public dialogRef: MatDialogRef<EditTaskComponent>,
    private spinner: NgxSpinnerService,
    private manager: ManagerService,
    private jwt: JwtService,
    @Inject(MAT_DIALOG_DATA) public task: TaskModel
  ) {
    this.managerId = this.jwt.getId();
    this.getTeam();
  }

  public onNoClick(): void {
    this.dialogRef.close(this.task);
  }

  public onSaveCLick(): void {
    this.spinner.show();
    this.dialogRef.close(this.task);
  }

  public writeAssignee(event) {
    const assignee = this.myTeam.filter(x => x.id === event.source.value)[0];
    this.task.assignee = assignee.fName + ' ' + assignee.lName;
  }

  public onCloseCLick(): void {
    this.dialogRef.close();
  }

  private getTeam() {
    this.spinner.show();
    this.manager.getMyTeam(this.managerId).subscribe(
      res => {
        this.myTeam = (res as TeamModel).team;
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
      }
    );
  }
}
