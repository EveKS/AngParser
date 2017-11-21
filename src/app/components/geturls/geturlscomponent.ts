import { Component, OnInit, Inject, OnDestroy, trigger, state, transition, style, animate } from '@angular/core';
import { NgModel, NgForm } from '@angular/forms';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { OkResult } from "../../shared/models/ok.result.interface";
import { StartParametrs } from "../../shared/models/start.parametrs.class";

@Component({
  selector: 'get-urls',
  templateUrl: './geturls.component.html'
})
export class GetUrlsComponent {
  messages: string[];
  continue: boolean;
  message: string;
  count: number;

  private _id: string;

  constructor(private _http: Http, @Inject('BASE_URL') private _baseUrl: string) {
    this.messages = [];
    this.continue = false;
    this.message = '';
    this._id = '';
    this.count = 0;
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

    let start: StartParametrs = Object.assign(new StartParametrs(), {
      message: this.message,
      count: this.count
    });

    const body = JSON.stringify(start);

    this._http.post(this._baseUrl + 'api/urlparser/get-urls', body, { headers: headers })
      .subscribe(result => {
        let ok = result.json() as OkResult;

        if (ok.ok === 'ok') {
          this._id = ok.id;
          this.messages.push(...ok.messages);
        }

        this.continue = false;
      }, error => {
        console.log('ERROR');
        console.error(error);
        this.continue = false;
      });
  }
}
