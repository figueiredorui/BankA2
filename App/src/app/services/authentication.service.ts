import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/delay';

@Injectable()
export class AuthenticationService {

    protected authEndPoint: string = 'authentication/signIn.json';

    constructor(private http: Http) { }

    // to check the login credentials
    public login(parameter?: any): Observable<any> {
        return this.http.get(this.authEndPoint)
            .map(res => {
                if (res) {
                 //   localStorage.setItem('auth_token', res.auth_token);
                    return res;
                }
            }).catch(err => err);
    }

    // to logou from the system
    public logout(): void {
        localStorage.removeItem('auth_token');
    }

    // to check the login status
    public checkLogin(): boolean {
        return !!localStorage.getItem('auth_token');
    }
}
