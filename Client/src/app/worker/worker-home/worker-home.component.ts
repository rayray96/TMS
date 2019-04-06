import { Component, OnInit } from '@angular/core';
import { NavBarService } from 'src/app/services';

@Component({
  selector: 'app-worker-home',
  templateUrl: './worker-home.component.html',
  styleUrls: ['./worker-home.component.css']
})
export class WorkerHomeComponent implements OnInit {

  constructor(public nav: NavBarService) { }

  ngOnInit() {
    this.nav.show();
  }

}
