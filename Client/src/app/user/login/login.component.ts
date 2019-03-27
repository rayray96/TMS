import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { JwtService } from 'src/app/services/jwt.service';
import { NgxSpinnerService } from 'ngx-spinner'


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {

  formModel = {
    UserName: '',
    Password: ''
  }

  constructor(private service: UserService,
    private router: Router,
    private toastr: ToastrService,
    private jwt: JwtService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    if (this.jwt.getAccessToken() != null)
      this.router.navigateByUrl('/home');
  }

  onSubmit(form: NgForm) {
    this.spinner.show();
    this.service.login(form.value).subscribe(
      (res: any) => {
        this.jwt.persistAccessToken(res.accessToken);
        this.service.setUserRole(res.role);
        this.router.navigateByUrl('/home');
        this.spinner.hide();
      },
      err => {
        if (err.status == 400 || err.status == 404) {
          this.toastr.error('Incorrect username or password', 'Authentication failed');
        }
        else {
          this.toastr.error('You cannot log in', 'Authentication failed');
          console.log(err);
        }
        this.spinner.hide();
      }
    );
  }
}
