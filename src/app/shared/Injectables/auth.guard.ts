import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { UserService } from "../../shared/services/user.service";

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private _user: UserService, private _router: Router) { }

  canActivate() {
    if (!this._user.isLoggedIn()) {
      this._router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
