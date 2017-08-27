import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { ThreadData } from '../models/Thread';

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

    private handleError(error: any): Promise<any> {
        console.error('Error occured while fething data: ', error);
        return Promise.reject(error.message || error);
    }
}
