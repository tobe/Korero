import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { ThreadData, Thread } from '../models/thread';
import { Reply } from '../models/reply';

import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class ThreadService {
    private endpoint = 'api/thread';

    constructor(private http: Http) {
    }

    getThreads(page: number): Promise<ThreadData> {
        return this.http.get(`${this.endpoint}/p/${page}`)
            .toPromise()
            .then(response => response.json() as ThreadData)
            .catch(this.handleError);
    }

    getThread(id: number): Promise<Thread> {
        return this.http.get(`${this.endpoint}/${id}`)
            .toPromise()
            .then(response => response.json() as Thread)
            .catch(this.handleError);
    }

    getReplies(id: number, page: number): Promise<Reply[]> {
        return this.http.get(`${this.endpoint}/r/${id}/page/${page}`)
            .toPromise()
            .then(response => response.json() as Reply[])
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('Error occured while fething data: ', error);
        return Promise.reject(error.message || error);
    }
}
