import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { UserModel } from '../models';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavBarService } from '../services/nav-bar.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})

export class HomeComponent implements OnInit {

  userDetails: UserModel;

  constructor(private router: Router,
     private service: UserService,
      private spinner: NgxSpinnerService,
      public nav: NavBarService) { }

  ngOnInit() {
    this.nav.show();
    this.spinner.show();
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = (res as UserModel);
        this.spinner.hide();
      },
      err => {
        console.log(err);
        this.spinner.hide();
      },
    );
  }
}
