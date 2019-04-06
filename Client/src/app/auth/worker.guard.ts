import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtService } from '../services';

@Injectable({
  providedIn: 'root'
})
export class WorkerGuard implements CanActivate {

  constructor(
    private jwt: JwtService,
    private router: Router
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.jwt.getRole() === 'Worker') {
      return true;
    }
    this.router.navigate(['/']);
    return false;
  }
}
