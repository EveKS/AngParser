import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

import { MainComponent } from "./components/main/main.component";
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import { FindEmailsComponent } from "./components/findemails/findemails.component";

import { FormsModule } from '@angular/forms';
import { HttpModule, XHRBackend } from '@angular/http';
import { HomeComponent } from "./components/home/home.component";
import { LoginFormComponent } from "./components/account/login-form/login-form.component";
import { RegistrationFormComponent } from "./components/account/registration-form/registration-form.component";

import { AuthenticateXHRBackend } from "./authenticate-xhr.backend";

import { UserService } from "./shared/services/user.service";
import { AuthGuard } from "./shared/Injectables/auth.guard";

import { myFocus } from "./directives/focus.directive";
import { SpinnerComponent } from "./spinner/spinner.component";
import { EmailValidator } from "./directives/email.validator.directive";
import { GetUrlsComponent } from "./components/geturls/geturlscomponent";

const appRoutes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },

  { path: 'find-emails', component: FindEmailsComponent, canActivate: [AuthGuard] },
  { path: 'get-urls', component: GetUrlsComponent, canActivate: [AuthGuard] },

  { path: 'register', component: RegistrationFormComponent },
  { path: 'login', component: LoginFormComponent },

  { path: '**', redirectTo: 'home' }
];

@NgModule({
  providers: [
    UserService, AuthGuard,
    { provide: XHRBackend, useClass: AuthenticateXHRBackend },
    { provide: 'BASE_URL', useFactory: getBaseUrl }
  ],
  declarations: [
    MainComponent,
    NavMenuComponent,
    FindEmailsComponent,
    GetUrlsComponent,
    HomeComponent,

    EmailValidator,
    RegistrationFormComponent,
    LoginFormComponent,

    myFocus, SpinnerComponent
  ],
  exports: [myFocus, SpinnerComponent],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    CommonModule,
    RouterModule.forRoot(appRoutes)
  ],
  bootstrap: [MainComponent]
})
export class AppModule { }

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}
