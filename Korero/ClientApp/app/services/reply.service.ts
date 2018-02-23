import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { Reply, ReplyData } from '../models/reply';

@Injectable()
export class ReplyService {
    private endpoint = '/api/thread';

    constructor(private _http: HttpClient) { }

    getReplies(id: number, page: number): Observable<ReplyData> {
        return this._http.get<ReplyData>(`${this.endpoint}/replies/${id}/page/${page}`)
            .pipe(
            catchError(this.handleError<ReplyData>('getReplies'))
            );
    }

    addReply(id: number, reply: Reply): Observable<Response> {
        return this._http.post<Response>(`${this.endpoint}/${id}`, reply)
            .pipe(
            catchError(this.handleError<Response>('addReply'))
            );
    }

    updateReply(reply: Reply): Observable<Response> {
        return this._http.put<Response>(`/api/reply/${reply.id}`, reply)
            .pipe(
            catchError(this.handleError<Response>('updateReply'))
            );
    }

    deleteReply(id: number): Observable<Response> {
        return this._http.delete<Response>(`/api/reply/${id}`)
            .pipe(
            catchError(this.handleError<Response>('deleteReply'))
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
