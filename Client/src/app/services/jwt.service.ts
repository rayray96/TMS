import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import * as jwt_decode from 'jwt-decode';

@Injectable()
export class JwtService {

  constructor(private http: HttpClient) { }
  readonly BaseURI = 'https://localhost:44360/api';

  persistAccessToken(accessToken: string): void {
    localStorage.setItem('accessToken', accessToken);
  }

  getAccessToken(): string {
    return localStorage.getItem('accessToken');
  }

  clearAccessToken(): void {
    localStorage.removeItem('accessToken');
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
  getRefreshToken(): string {
    return localStorage.getItem('refreshToken');
  }
  // TODO: Add posibility to use refresh token.
  persistRefreshToken(refreshToken: string): void {
    localStorage.setItem('refreshToken', refreshToken);
  }
  // TODO: Add posibility to use refresh token.
  clearRefreshToken(): void {
    localStorage.removeItem('refreshToken');
  }
  // TODO: Add posibility to use refresh token.
  refreshToken(refreshToken: string) {
    return this.http.post<any>(this.BaseURI + '/account/' + refreshToken + '/refresh', {});
  }
}
