import { Injectable } from '@angular/core';
import { UserModel } from '../models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { RoleModel } from '../models/role-model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  readonly BaseURI = 'https://localhost:44360/api/admin';
  currentUser: UserModel;

  constructor(private http: HttpClient) { }

  public getAll(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(this.BaseURI);
  }

  public get(id: string): Observable<UserModel> {
    const path = `/${id}`;
    return this.http.get<UserModel>(this.BaseURI + path);
  }

  public updateRole(role: RoleModel, id: string) {
    const path = `/${id}`;
    return this.http.put(this.BaseURI + path, role);
  }
}
