import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { User } from '../models/user';

import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class AuthService {
    private endpoint = 'api/user';

    constructor(private http: Http) {}

    // Returns the currently logged in user.
    getUser(): Promise<User> {
        return this.http.get(this.endpoint)
            .toPromise()
            .then(response => response.json() as User)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('Error occured while fething data: ', error);
        return Promise.reject(error.message || error);
    }
}
