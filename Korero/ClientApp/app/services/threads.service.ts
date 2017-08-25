import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class ThreadsService {
    constructor(private http: Http) {
        console.log('ThreadsService loaded');
    }
}
