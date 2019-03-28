import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserService, JwtService } from '../services';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { UserModel } from '../models';

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
    private service: UserService, 
    private spinner: NgxSpinnerService, 
    private router: Router,
    private jwt: JwtService) {}

    ngOnInit() {
    this.spinner.show();
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = (res as UserModel);
        this.spinner.hide();
      },
      err => {
        console.log(err)
        this.spinner.hide();
      },
    );
  }

  onLogout() {
    this.spinner.show();
    this.jwt.clearAccessToken();
    this.service.removeRoleFromStorage();
    this.router.navigate(['/user/login'])
    this.spinner.hide();
  }
}
