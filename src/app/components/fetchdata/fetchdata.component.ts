import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';

@Component({
    selector: 'finf-emails',
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

    sendMessage(): void {
        this.continue = true;

        let headers = new Headers();
        headers.append('Content-Type', 'application/json;charset=utf-8');
        const body = JSON.stringify(this.message);

        this._http.post(this._baseUrl + 'api/emailparser/start', body, { headers })
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
            });
    }

    AngParsers() {
        this._http.get(this._baseUrl + 'api/emailparser/get-emails')
            .subscribe(result => {
                let messages = result.json() as Message;

                if (messages.continue) {
                    this.messages.push(...messages.emails);
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
