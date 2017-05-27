import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

@Injectable()
export class UserService {
    constructor(private http: Http) { }

    register(email: string, password: string, confirmPassword: string) {
        return this.http.post('/api/account/register', { email: email, password: password, confirmPassword: confirmPassword });
    }
}