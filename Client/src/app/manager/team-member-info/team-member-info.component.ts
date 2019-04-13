import { Component, OnInit, OnDestroy } from '@angular/core';
import { ManagerService } from 'src/app/services';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-team-member-info',
  templateUrl: './team-member-info.component.html',
  styleUrls: ['./team-member-info.component.css']
})
export class TeamMemberInfoComponent implements OnInit, OnDestroy {

  constructor(
    private manager: ManagerService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService) { }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    this.manager.currentPerson = undefined;
  }

  deleteMember() {
    this.spinner.show();
    this.manager.deleteFromTeam(this.manager.currentPerson.id).subscribe(
      (res: any) => {
        this.manager.needCheck = true;
        this.manager.currentPerson = undefined;
        this.spinner.hide();
        this.toastr.success(res.message);
      },
      (err: any) => {
        this.spinner.hide();
        this.toastr.warning(err.error);
      }
    );
  }
}
