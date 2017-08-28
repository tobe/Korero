import { Component, OnInit } from '@angular/core';

import { ThreadService } from '../../services/thread.service';

import { ThreadData } from '../../models/Thread';

@Component({
    selector: 'app-forum',
    templateUrl: './forum.component.html',
    styleUrls: ['./forum.component.css'],
    providers: [ThreadService]
})
export class ForumComponent implements OnInit {
    /* https://stackoverflow.com/questions/35763730/difference-between-constructor-and-ngoninit */

    public threads: ThreadData; // API return
    public loading = false;
    public total = 0;
    public page = 1;
    public limit = 1;

    // Same as private threadservice;  threadservice = ThreadService... Some neat DI
    constructor(private threadService: ThreadService) { }

    ngOnInit(): void {
        this.getThreads();
    }

    getThreads(): void {
        this.loading = true;
        this.threadService.getThreads(this.page).then(threads => {
            this.threads = threads;
            this.total   = threads.total;
            this.loading = false;
        });
    }

    goToPage(n: number): void {
        this.page = n;
        this.getThreads();
    }

    onNext(): void {
        this.page++;
        this.getThreads();
    }

    onPrev(): void {
        this.page--;
        this.getThreads();
    }
}
