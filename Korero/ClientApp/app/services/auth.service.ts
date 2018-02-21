import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { User } from '../models/user';

@Injectable()
export class AuthService {
    private endpoint = '/api/user/';

    constructor(private _http: HttpClient) { }

    getUser(): Observable<User> {
        return this._http.get<User>(this.endpoint)
            .pipe(
                catchError(this.handleError<User>('getUser'))
            );
    }

    /**
     * Handle Http operation that failed.
     * Let the app continue.
     * @param operation - name of the operation that failed
     * @param result - optional value to return as the observable result
     */
    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            // TODO: send the error to remote logging infrastructure
            console.error(error); // log to console instead

            // Don't keep the app running, throw an exception
            return Observable.throw(null);
        };
    }
}
