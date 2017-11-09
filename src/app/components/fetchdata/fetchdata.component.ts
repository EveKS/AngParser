import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { NgModel, NgForm } from '@angular/forms';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Component({
  selector: 'find-emails',
  templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
  messages: string[];
  continue: boolean;
  message: string;

  constructor(private _http: Http, @Inject('BASE_URL') private _baseUrl: string) {
    this.messages = [];
    this.continue = false;
    this.message = '';
  }

  private getHeaders(): Headers {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);

    return headers;
  }

  sendMessage(): void {
    this.continue = true;

    let headers = this.getHeaders();
    headers.append('Content-Type', 'application/json;charset=utf-8');
    const body = JSON.stringify(this.message);

    this._http.post(this._baseUrl + 'api/emailparser/start', body, { headers: headers })
      .subscribe(result => {
        let ok = result.json() as Ok;

        if (ok.ok === 'ok') {
          this.AngParsers();
        } else {
          this.continue = false;
        }
      }, error => {
        console.log('ERROR');
        console.error(error);
        this.continue = false;
      });
  }

  AngParsers() {
    let headers = this.getHeaders();

    this._http.get(this._baseUrl + 'api/emailparser/get-emails', { headers: headers })
      .subscribe(result => {
        let messages = result.json() as Message;

        this.messages.push(...messages.emails);

        if (messages.continue) {
          setTimeout(() => { this.AngParsers(); }, 250);
        }

        this.continue = messages.continue;
      }, error => {
        console.log('ERROR');
        console.error(error);
        this.continue = false;
      });
  }
}

interface Ok {
  ok: string;
}

interface Message {
  emails: string[];
  continue: boolean;
}
