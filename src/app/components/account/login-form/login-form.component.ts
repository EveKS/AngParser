import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Subscription } from "rxjs/Subscription";
import { Credentials } from "../../../shared/models/credentials.interface";
import { UserService } from "../../../shared/services/user.service";
import { AuthGuard } from "../../../shared/Injectables/auth.guard";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css'],
  /*providers: [UserService, AuthGuard]*/
})
export class LoginFormComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  credentials: Credentials = { email: '', password: '' };
  returnUrl: string;

  constructor(private _userService: UserService, private _router: Router, private _activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // subscribe to router event
    this.subscription = this._activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param['brandNew'];
        this.credentials.email = param['email'];
        this.returnUrl = param['returnUrl'];
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  login({ value, valid }: { value: Credentials, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid) {
      this._userService.login(value.email, value.password)
        /*.finally(() => this.isRequesting = false)*/
        .subscribe(
        result => {
          if (result) {
            if (this.returnUrl !== undefined || this.returnUrl != null) {
              this._router.navigate([this.returnUrl]);
            } else {
              this._router.navigate(['/home']);
            }
          }

          this.isRequesting = false;
        },
        error => {
          if (error.code === 500) {
            this.errors = 'Отсутствует интернет соединение';
          } else {
            this.errors = error;
          }

          this.isRequesting = false;
        });
    } else {
      this.submitted = false;
      this.isRequesting = false;
    }
  }
}
