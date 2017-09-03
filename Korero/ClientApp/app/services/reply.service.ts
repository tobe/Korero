import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { ReplyData, Reply } from '../models/reply';

import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class ReplyService {
    private endpoint = 'api/thread';

    constructor(private http: Http) {}

    getReplies(id: number, page: number): Promise<ReplyData> {
        return this.http.get(`${this.endpoint}/r/${id}/page/${page}`)
            .toPromise()
            .then(response => response.json() as ReplyData)
            .catch(this.handleError);
    }

    addReply(id: number, reply: Reply): Promise<Response> {
        return this.http.post(`${this.endpoint}/${id}`, reply)
            .toPromise()
            .then(response => response.json() as Response)
            .catch(this.handleError);
    }

    updateReply(id: number, reply: Reply): Promise<Response> {
        return this.http.put(`${this.endpoint}/r/${id}`, reply)
            .toPromise()
            .then(response => response as Response)
            .catch(this.handleError);
    }

    deleteReply(id: number): Promise<Response> {
        return this.http.delete(`${this.endpoint}/r/${id}`)
            .toPromise()
            .then(response => response as Response)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('Error occured while fething data: ', error);
        return Promise.reject(error.message || error);
    }
}
