import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import * as jwt_decode from 'jwt-decode';

@Injectable()
export class JwtService {
  readonly BaseURI = 'https://localhost:44360/api';

  constructor(private http: HttpClient) { }

  persistData(accessToken: string, refreshToken: string, role: string, id: string): void {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    localStorage.setItem('role', role);
    localStorage.setItem('id', id);
  }

  clearData(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('role');
    localStorage.removeItem('id');
  }

  getAccessToken(): string {
    return localStorage.getItem('accessToken');
  }

  getRole(): string {
    return localStorage.getItem('role');
  }

  getId(): string {
    return localStorage.getItem('id');
  }

  getRefreshToken(): string {
    return localStorage.getItem('refreshToken');
  }

  // TODO: Add posibility to use refresh token.
  getTokenExpirationDate(token: string): Date {
    const decoded = jwt_decode(token);

    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }
  // TODO: Add posibility to use refresh token.
  isTokenExpired(token?: string): boolean {
    if (!token) token = this.getAccessToken();
    if (!token) return true;

    const date = this.getTokenExpirationDate(token);
    if (date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }
  // TODO: Add posibility to use refresh token.
  refreshToken(refreshToken: string) {
    return this.http.post<any>(this.BaseURI + '/account/' + refreshToken + '/refresh', {});
  }
}
