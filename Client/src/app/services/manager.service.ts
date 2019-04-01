import { Injectable } from '@angular/core';
import { PersonModel } from '../models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TeamModel } from '../models/team-model';

@Injectable({
  providedIn: 'root'
})
export class ManagerService {
  readonly BaseURI = 'https://localhost:44360/api';
  currentPerson: PersonModel;

  constructor(private http: HttpClient) { }

  public getMyTeam(id: string): Observable<TeamModel> {
    const path = `/${id}`;
    return this.http.get<TeamModel>(this.BaseURI + '/team' + path);
  }

  public getPossibleMembers(): Observable<PersonModel[]> {
    return this.http.get<PersonModel[]>(this.BaseURI + '/team/possibleMembers');
  }

  public createTeam(id: string, teamName: string) {
    const path = `/${id}`;
    const body = { teamName };
    return this.http.post(this.BaseURI + '/team' + path, body);
  }

  public deleteFromTeam(id: string) {
    const path = `/${id}`;
    return this.http.delete(this.BaseURI + '/team' + path);
  }

  public addMembers(ids: string[]) {
    return this.http.post(this.BaseURI + '/team/addMembers', ids);
  }
}
