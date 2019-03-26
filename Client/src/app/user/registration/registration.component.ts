import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner'

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {

  constructor(public service: UserService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.service.formModel.reset();
  }

  onSubmit() {
    this.spinner.show();
    this.service.register().subscribe(
      (res: any) => {
        this.service.formModel.reset();
        this.toastr.success('New user created!', 'Registration successful.');
        this.spinner.hide();
      },
      (err: any) => {
        if (err.status == 400) {
          this.toastr.error(err.error.message, 'Registration failed');
        }
        else {
          console.log(err);
          this.toastr.error('Unable to create new user', 'Registration failed');
        }
        this.spinner.hide();
      });
  }
}
