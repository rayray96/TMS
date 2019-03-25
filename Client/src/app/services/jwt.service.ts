import { Injectable } from '@angular/core';
import { JwtPayload } from '../models';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import * as jwt_decode from 'jwt-decode';

@Injectable()
export class JwtService {

  private token = {
    accessToken: null as string,
    refreshToken: null as string
  };

  constructor(private window: Window, private http: HttpClient) { }
  readonly BaseURI = 'http://localhost:61738/api';

  public persistToken(accessToken: string, refreshToken: string): void {
    this.token.accessToken = accessToken;
    this.token.refreshToken = refreshToken;
    console.log('token string parsed and saved to local storage');
    console.log(this.token);
    this.window.localStorage.setItem('accessToken', accessToken);
    this.window.localStorage.setItem('refreshToken', refreshToken);
  }

  getTokenExpirationDate(token: string): Date {
    const decoded = jwt_decode(token);

    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  isTokenExpired(token?: string): boolean {
    if (!token) token = this.getAccessToken();
    if (!token) return true;

    const date = this.getTokenExpirationDate(token);
    if (date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }

  getAccessToken(): string {
    return this.token.accessToken;
  }

  getRefershToken(): string {
    return this.token.refreshToken;
  }

  clearToken(): void {
    this.token.accessToken = null;
    this.token.refreshToken = null;
    this.window.localStorage.removeItem('accessToken');
    this.window.localStorage.removeItem('refreshToken');
  }

  // public fetchToken(): { accessToken: string, refreshToken: string } {
  //   if (this.token.accessToken) {
  //     return this.token;
  //   }

  //   const currentToken = this.window.localStorage.getItem('accessToken');

  //   if (!currentToken) {
  //     return {} as any;
  //   }

  //   var tokens = this.refreshTokens(this.token.refreshToken)
  //   this.token.accessToken = tokens.accessToken;
  //   this.token.refreshToken = tokens.refreshToken;

  //   return this.token;
  // }

  refreshTokens(refreshToken: string) {
    return this.http.post(this.BaseURI + '/account/' + refreshToken + '/resfresh', refreshToken).subscribe(
      (res: any) => {
        this.jwt.persistToken(res.accessToken, res.refreshToken)
        this.router.navigateByUrl('/home');
      },
      err => {
        if (err.status == 400)
          this.toastr.error('Incorrect username or password', 'Authentication failed');
        else
          console.log(err);
      }
    );
}
