import { Injectable } from '@angular/core';
import { TaskModel, CreateTaskModel, StatusModel, EditStatusModel } from '../models';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  readonly BaseURI = '/api/task';
  currentTask: TaskModel;
  needCheck = false;

  constructor(private http: HttpClient) { }

  public getTasksOfManager(id: string): Observable<TaskModel[]> {
    const path = `/${id}`;
    return this.http.get<TaskModel[]>(this.BaseURI  + '/managerTasks' + path);
  }

  public getTasksOfWorker(id: string): Observable<TaskModel[]> {
    const path = `/${id}`;
    return this.http.get<TaskModel[]>(this.BaseURI  + '/workerTasks' + path);
  }

  public getStatuses(): Observable<StatusModel[]> {
    return this.http.get<StatusModel[]>(this.BaseURI  + '/statuses');
  }

  public createTask(task: CreateTaskModel): Observable<CreateTaskModel> {
    return this.http.post<CreateTaskModel>(this.BaseURI, task);
  }

  public updateTask(id: number, task: CreateTaskModel): Observable<CreateTaskModel> {
    const path = `/${id}`;
    return this.http.put<CreateTaskModel>(this.BaseURI + path, task);
  }

  public updateStatus(id: string, status: EditStatusModel): Observable<EditStatusModel> {
    const path = `/${id}`;
    return this.http.put<EditStatusModel>(this.BaseURI + path + '/status', status);
  }

  public deleteTask(id: number) {
    const path = `/${id}`;
    return this.http.delete(this.BaseURI + path);
  }
}
