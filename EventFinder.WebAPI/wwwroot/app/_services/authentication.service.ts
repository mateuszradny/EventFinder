import { Injectable, OnInit } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map'

import { User } from '../_models/user'

@Injectable()
export class AuthenticationService implements OnInit {
    constructor(private http: Http) { }

    private currentUser = new BehaviorSubject<User>(this.retriveUserFromLocalStorage());

    ngOnInit() {
        this.setUserFromLocalStorageAsCurrent();
    }

    getCurrentUser() {
        return this.currentUser.asObservable();
    }

    login(email: string, password: string) {
        return this.http.post('/api/account/token', { email: email, password: password })
            .map((response: Response) => {
                let user = response.json();
                if (user && user.token) {
                    localStorage.setItem('currentUser', JSON.stringify(user));
                    this.setUserFromLocalStorageAsCurrent();
                }
            });
    }

    logout() {
        localStorage.removeItem('currentUser');
        this.setUserFromLocalStorageAsCurrent();
    }

    getAuthHeaders() {
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser && currentUser.token) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
            return new RequestOptions({ headers: headers });
        }

        return null;
    }

    private retriveUserFromLocalStorage() {
        let token = localStorage.getItem('currentUser');
        if (token) {
            let values = this.parseToken(token);
            return {
                id: +values["sub"],
                email: values["email"],
                roles: values["roles"] || []
            };
        }

        return null;
    }

    private setUserFromLocalStorageAsCurrent() {
        this.currentUser.next(this.retriveUserFromLocalStorage());
    }

    private parseToken(token: string): any {
        var base64 = token.split('.')[1].replace('-', '+').replace('_', '/');
        return JSON.parse(window.atob(base64));
    }
}