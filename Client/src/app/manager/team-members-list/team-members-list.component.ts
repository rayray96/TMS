import { Component, OnInit, ViewChild, DoCheck, AfterViewInit, AfterContentChecked, SimpleChanges } from '@angular/core';
import { MatSort, MatPaginator, MatTableDataSource, MatListOption, MatSortable } from '@angular/material';
import { NgxSpinnerService } from 'ngx-spinner';
import { PersonModel, TeamModel, TeamMembersModel } from 'src/app/models';
import { JwtService, ManagerService } from 'src/app/services';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-team-members-list',
  templateUrl: './team-members-list.component.html',
  styleUrls: ['./team-members-list.component.css']
})
export class TeamMembersListComponent implements OnInit, DoCheck {
  displayedColumns: string[] = ['fName', 'lName', 'email'];
  possibleMembers: PersonModel[];
  newMembers: TeamMembersModel;
  oldTeamName: string;
  newTeamName: string;
  managerId: string;
  dataSource;

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
    console.log('ngOnInit');
  }

  ngDoCheck() {
    if (this.manager.needCheck) {
      this.getTeam();
      console.log('ngDoCheck');
    }
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  createTeam() {
    this.spinner.show();
    this.manager.createTeam(this.managerId, this.newTeamName).subscribe(
      res => {
        this.manager.needCheck = true;
        this.manager.currentPerson = undefined;
        this.spinner.hide();
        this.toastr.success('The new team has created!');
      },
      err => {
        this.spinner.hide();
        console.log(err);
        this.toastr.error(err.error);
      }
    );
  }

  updateTeamName() {
    this.spinner.show();
    this.manager.updateTeamName(this.managerId, this.newTeamName).subscribe(
      (res: any) => {
        this.manager.needCheck = true;
        this.newTeamName = res.newName;
        this.spinner.hide();
        this.toastr.success('The team name has updated!');
      },
      (err: any) => {
        this.spinner.hide();
        if (err.error.errors.TeamName) {
          this.toastr.error(err.error.errors.TeamName);
        }
        else {
          this.toastr.error(err.error.errors.TeamName);
        }
        console.log(err);
      }
    );
  }

  onSelect(personModel: PersonModel): void {
    this.manager.currentPerson = personModel;
  }

  onGroupsChange(options: MatListOption[]) {
    const members = options.map(o => o.value);
    this.newMembers = { members } as TeamMembersModel;
  }

  addNewMembers() {
    this.spinner.show();
    this.manager.addMembers(this.managerId, this.newMembers).subscribe(
      (res: any) => {
        this.manager.needCheck = true;
        this.spinner.hide();
        this.toastr.success(res.message);
      },
      (err: any) => {
        if (!this.newMembers) {
          this.spinner.hide();
          console.log(err);
          this.toastr.warning("You have not chosen new members");
        }
        else {
          this.spinner.hide();
          console.log(err);
          this.toastr.error("Cannot to add new members to the team");
        }
      }
    );
  }

  getPossibleMembers() {
    this.spinner.show();
    this.manager.getPossibleMembers().subscribe(
      res => {
        this.possibleMembers = res as PersonModel[];
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
      }
    );
  }

  private getTeam() {
    this.spinner.show();
    this.manager.needCheck = false;
    this.manager.getMyTeam(this.managerId).subscribe(
      res => {
        this.dataSource = new MatTableDataSource((res as TeamModel).team);

        setTimeout(() => {
          this.sort.sort(<MatSortable>({ id: 'fName', start: 'asc' }));
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        });

        this.oldTeamName = res.teamName;
        this.newTeamName = res.teamName;
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        console.log(err);
      }
    );
  }
}
