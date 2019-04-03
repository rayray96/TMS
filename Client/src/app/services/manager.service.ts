import { Injectable } from '@angular/core';
import { PersonModel, TeamNameModel, TeamMembersModel } from '../models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TeamModel } from '../models/team-model';

@Injectable({
  providedIn: 'root'
})
export class ManagerService {
  readonly BaseURI = 'https://localhost:44360/api/team';
  currentPerson: PersonModel;
  needCheck = false;

  constructor(private http: HttpClient) { }

  public getMyTeam(id: string): Observable<TeamModel> {
    const path = `/${id}`;
    return this.http.get<TeamModel>(this.BaseURI + path);
  }

  public getPossibleMembers(): Observable<PersonModel[]> {
    return this.http.get<PersonModel[]>(this.BaseURI + '/possibleMembers');
  }

  public createTeam(id: string, teamName: string) {
    const path = `/${id}`;
    const model = { teamName } as TeamNameModel;
    return this.http.post(this.BaseURI + path, model);
  }

  public updateTeamName(id: string, teamName: string) {
    const path = `/${id}`;
    const model = { teamName } as TeamNameModel;
    return this.http.put(this.BaseURI + path, model);
  }

  public deleteFromTeam(id: string) {
    const path = `/${id}`;
    return this.http.delete(this.BaseURI + path);
  }

  public addMembers(id: string, members: TeamMembersModel) {
    const path = `/${id}`;
    return this.http.post(this.BaseURI + '/addMembers' + path, members);
  }
}
