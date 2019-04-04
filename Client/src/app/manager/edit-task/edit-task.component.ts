import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatListOption } from '@angular/material';
import { CreateTaskModel, PersonModel, TeamModel, TaskModel } from 'src/app/models';
import { NgxSpinnerService } from 'ngx-spinner';
import { ManagerService, JwtService, TaskService } from 'src/app/services';

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
    @Inject(MAT_DIALOG_DATA) public task: TaskModel,
    private spinner: NgxSpinnerService,
    private manager: ManagerService,
    private taskService: TaskService,
    private jwt: JwtService
  ) {
    this.task = this.taskService.currentTask;
    this.managerId = this.jwt.getId();
    this.getTeam();
  }

  public onNoClick(): void {
    this.dialogRef.close(this.task);
  }

  public onSaveCLick(): void {
    this.dialogRef.close(this.task);
  }

  public onCloseCLick(): void {
    this.dialogRef.close();
  }

  onGroupsChange(options: MatListOption[]) {
    const members = options.map(o => o.value);
    //this.newMembers = { members } as TeamMembersModel;
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
