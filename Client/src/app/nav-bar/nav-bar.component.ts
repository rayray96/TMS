import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserService, JwtService } from '../services';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { UserModel } from '../models';
import { NavBarService } from '../services/nav-bar.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent {
  userDetails: UserModel;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );

  constructor(private breakpointObserver: BreakpointObserver, 
    private spinner: NgxSpinnerService, 
    private router: Router,
    private jwt: JwtService,
    public nav: NavBarService) {}

  onLogout() {
    this.spinner.show();
    this.jwt.clearData();
    this.router.navigate(['/user/login']);
    this.spinner.hide();
  }
}
