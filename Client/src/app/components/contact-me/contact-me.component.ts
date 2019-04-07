import { Component, OnInit } from '@angular/core';
import { NavBarService } from 'src/app/services';

@Component({
  selector: 'app-contact-me',
  templateUrl: './contact-me.component.html',
  styleUrls: ['./contact-me.component.css']
})
export class ContactMeComponent implements OnInit {
  
  constructor(public nav: NavBarService) { }

  ngOnInit() {
    this.nav.show();
  }
}
