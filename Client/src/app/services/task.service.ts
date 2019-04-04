import { Injectable } from '@angular/core';
import { TaskModel, CreateTaskModel } from '../models';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  readonly BaseURI = 'https://localhost:44360/api/task';
  currentTask: TaskModel;
  needCheck = false;

  constructor(private http: HttpClient) { }

  public getTasksOfMyTeam(id: string): Observable<TaskModel[]> {
    const path = `/${id}`;
    return this.http.get<TaskModel[]>(this.BaseURI + '/teamTasks' + path);
  }

  public createTask(task: CreateTaskModel): Observable<CreateTaskModel> {
    return this.http.post<CreateTaskModel>(this.BaseURI, task);
  }

  public updateTask(id: number, task: CreateTaskModel): Observable<CreateTaskModel> {
    const path = `/${id}`;
    return this.http.put<CreateTaskModel>(this.BaseURI + path, task);
  }

  public deleteTask(id: number) {
    const path = `/${id}`;
    return this.http.delete(this.BaseURI + path);
  }

  // public getMyTeam(id: string): Observable<TeamModel> {
  //   const path = `/${id}`;
  //   return this.http.get<TeamModel>(this.BaseURI + '' + path);
  // }

  // public getPossibleMembers(): Observable<PersonModel[]> {
  //   return this.http.get<PersonModel[]>(this.BaseURI + '/possibleMembers');
  // }

  // public createTeam(id: string, teamName: string) {
  //   const path = `/${id}`;
  //   const model = { teamName } as TeamNameModel;
  //   return this.http.post(this.BaseURI + path, model);
  // }

  // public updateTeamName(id: string, teamName: string) {
  //   const path = `/${id}`;
  //   const model = { teamName } as TeamNameModel;
  //   return this.http.put(this.BaseURI + path, model);
  // }

  // public deleteFromTeam(id: string) {
  //   const path = `/${id}`;
  //   return this.http.delete(this.BaseURI + '/team' + path);
  // }

  // public addMembers(id: string, members: TeamMembersModel) {
  //   const path = `/${id}`;
  //   return this.http.post(this.BaseURI + '/team/addMembers' + path, members);
  // }
}
