import { Component, OnInit } from '@angular/core';
import { NavBarService } from '../services/nav-bar.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styles: []
})
export class UserComponent implements OnInit {

  constructor(public nav: NavBarService) { }

  ngOnInit() {
    this.nav.hide();
  }

}
