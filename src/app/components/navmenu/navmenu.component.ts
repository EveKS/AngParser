import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Subscription } from "rxjs/Subscription";
import { UserService }from "../../shared/services/user.service";

@Component({
  selector: 'nav-menu',
  templateUrl: './navmenu.component.html',
  styleUrls: ['./navmenu.component.css'],
  /*providers: [UserService]*/
})
export class NavMenuComponent implements OnInit, OnDestroy {
  subscription: Subscription;
  @Input() status: boolean;

  constructor(private _userService: UserService) {
  }

  logout() {
    this._userService.logout();
  }

  ngOnInit(): void {
    this.subscription = UserService.authNavStatus$.subscribe(status => {
      this.status = status;
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
