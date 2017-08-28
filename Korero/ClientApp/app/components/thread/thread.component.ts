import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import { ThreadService } from '../../services/thread.service';

import { Thread } from '../../models/thread';
import { Reply } from '../../models/reply';

@Component({
    selector: 'app-thread',
    templateUrl: './thread.component.html',
    styleUrls: ['./thread.component.css'],
    providers: [ThreadService]
})
export class ThreadComponent implements OnInit {
    // Will hold the data returned from the API
    public thread: Thread;
    public replies: Reply[];

    // Thread ID
    public id: number;

    // Pagination stuff
    public loading = false;
    public total = 0;
    public page = 1;
    public limit = 5;

    // Router stuff
    private sub: any;

    constructor(private threadService: ThreadService, private route: ActivatedRoute) { }

    ngOnInit(): void {
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id']; // (+) converts string 'id' to a number
        });

        this.getThread(this.id);
        //this.getReplies(this.id);
    }

    getReplies(): void {
        /*this.loading = true;
        this.threadService.getReplies(this.id).then(replies => {
            this.thread = thread;
            this.loading = false;
        });*/
    }

    getThread(id: number): void {
        this.threadService.getThread(this.id).then(thread => this.thread = thread);
    }

    goToPage(n: number): void {
        this.page = n;
        //this.getThreads();
    }

    onNext(): void {
        this.page++;
        //this.getThreads();
    }

    onPrev(): void {
        this.page--;
        //this.getThreads();
    }
}
