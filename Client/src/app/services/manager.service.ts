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

  public getMyTeam(): Observable<PersonModel[]> {
    return this.http.get<PersonModel[]>(this.BaseURI + '/team');
  }

  public getPossibleMembers(id: string): Observable<PersonModel[]> {
    const path = `/${id}`;
    return this.http.get<PersonModel[]>(this.BaseURI + '/team' + path);
  }

  public createTeam(id: string, team: TeamModel) {
    const path = `/${id}`;
    return this.http.post(this.BaseURI + '/team' + path, team);
  }

  public deleteFromTeam(id: string) {
    const path = `/${id}`;
    return this.http.delete(this.BaseURI + '/team' + path);
  }

  public addMembers(ids: string[]) {
    return this.http.post(this.BaseURI + '/team/addMembers', ids);
  }
}
