import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { ThreadData } from '../models/thread';

@Injectable()
export class ThreadService {
    private endpoint = '/api/thread/';

    constructor(private _http: HttpClient) { }

    getThreads(page: number): Observable<ThreadData> {
        return this._http.get<ThreadData>(`${this.endpoint}/page/${page}`)
            .pipe(
                catchError(this.handleError<ThreadData>('asd'))
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

            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }
}
