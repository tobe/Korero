import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import { ThreadService } from '../../services/thread.service';
import { AuthService } from '../../services/auth.service';

import { Thread } from '../../models/thread';
import { Reply } from '../../models/reply';
import { User } from '../../models/user';

@Component({
    selector: 'app-thread',
    templateUrl: './thread.component.html',
    styleUrls: ['./thread.component.css'],
    providers: [ThreadService, AuthService]
})
export class ThreadComponent implements OnInit, OnDestroy {
    // Will hold the data returned from the API
    public thread: Thread;
    public replies: Reply[];
    public user: User;

    // Thread ID
    public id: number;

    // Pagination stuff
    public total = 0;
    public page = 1;
    public limit = 5;

    // Router stuff
    private sub: any;

    constructor(
        private threadService: ThreadService,
        private route: ActivatedRoute,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id']; // (+) converts string 'id' to a number
        });

        this.getThread(this.id);
        this.getReplies();
        this.getCurrentUser();
    }

    ngOnDestroy(): void {
        // Remove the router subscription when the lifecycle ends
        this.sub.unsubscribe();
    }

    getCurrentUser(): void {
        this.authService.getUser().then(user => this.user = user);
    }

    getReplies(): void {
        this.threadService.getReplies(this.id, this.page).then(replies => {
            this.replies = replies.data;
            this.total = replies.total;
            console.log(this.replies);
        });
    }

    getThread(id: number): void {
        this.threadService.getThread(this.id).then(thread => this.thread = thread);
    }

    goToPage(n: number): void {
        this.page = n;
        this.getReplies();
    }

    onNext(): void {
        this.page++;
        this.getReplies();
    }

    onPrev(): void {
        this.page--;
        this.getReplies();
    }
}
