import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { ThreadData, Thread } from '../models/thread';

@Injectable()
export class ThreadService {
    private endpoint = '/api/thread';

    constructor(private _http: HttpClient) { }

    getThreads(page: number): Observable<ThreadData> {
        return this._http.get<ThreadData>(`${this.endpoint}/page/${page}`)
            .pipe(
                catchError(this.handleError<ThreadData>('getThreads'))
            );
    }

    getThread(id: number): Observable<Thread> {
        return this._http.get<Thread>(`${this.endpoint}/${id}`)
            .pipe(
                catchError(this.handleError<Thread>('getThread'))
            );
    }

    deleteThread(id: number): Observable<Response> {
        return this._http.delete<Response>(`${this.endpoint}/${id}`)
            .pipe(
                catchError(this.handleError<Response>('deleteThread'))
            );
    }

    createThread(thread: Thread): Observable<Thread> {
        return this._http.post<Thread>(`${this.endpoint}`, thread)
            .pipe(
                catchError(this.handleError<Thread>('createThread'))
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
