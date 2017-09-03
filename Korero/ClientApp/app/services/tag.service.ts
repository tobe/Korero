import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Tag } from '../models/tag';

import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class TagService {
    private endpoint = 'api/tag';

    constructor(private http: Http) {}

    getTags(): Promise<Tag[]> {
        return this.http.get(this.endpoint)
            .toPromise()
            .then(response => response.json() as Tag[])
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('Error occured while fething data: ', error);
        return Promise.reject(error.message || error);
    }
}
