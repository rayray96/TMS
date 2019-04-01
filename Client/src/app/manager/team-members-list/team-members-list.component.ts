import { Component, OnInit, ViewChild, DoCheck } from '@angular/core';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { ManagerService } from 'src/app/services/manager.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { PersonModel, TeamModel } from 'src/app/models';
import { JwtService } from 'src/app/services';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-team-members-list',
  templateUrl: './team-members-list.component.html',
  styleUrls: ['./team-members-list.component.css']
})
export class TeamMembersListComponent implements OnInit, DoCheck {
  displayedColumns: string[] = ['fName', 'lName', 'email'];
  dataSource;
  oldTeamName;
  newTeamName;
  managerId;

  constructor(private manager: ManagerService,
    private spinner: NgxSpinnerService,
    private jwt: JwtService,
    private toastr: ToastrService) {
    this.managerId = this.jwt.getId();
  }

  @ViewChild(MatSort)
  sort: MatSort;

  @ViewChild(MatPaginator)
  paginator: MatPaginator;

  ngOnInit() {
    this.getTeam();
  }

  ngDoCheck() {
    if (this.oldTeamName !== this.newTeamName)
      this.getTeam();
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  createTeam() {
    this.manager.createTeam(this.managerId, this.newTeamName).subscribe(
      res => {
        this.toastr.success('The new team has created!');
      },
      err => {
        console.log(err);
        this.toastr.error(err.error);
      }
    );
  }

  onSelect(personModel: PersonModel): void {
    this.manager.currentPerson = personModel;
  }

  private getTeam() {
    this.spinner.show();
    this.manager.getMyTeam(this.managerId).subscribe(
      res => {
        this.oldTeamName = res.teamName;
        this.newTeamName = res.teamName;
        this.dataSource = new MatTableDataSource((res as TeamModel).team);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        throw err;
      }
    );
  }
}
