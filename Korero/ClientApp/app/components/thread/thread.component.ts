import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationsService } from 'angular2-notifications';

import { ThreadService } from '../../services/thread.service';
import { ReplyService } from '../../services/reply.service';
import { AuthService } from '../../services/auth.service';

import { Thread } from '../../models/thread';
import { Reply } from '../../models/reply';
import { User } from '../../models/user';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'app-thread',
    templateUrl: './thread.component.html',
    styleUrls: ['./thread.component.css'],
    providers: [ReplyService, AuthService, ThreadService]
})
export class ThreadComponent implements OnInit, OnDestroy {
    // Will hold the data returned from the API
    public thread: Thread;
    public replies: Reply[];
    public user: User;

    // New reply to the thread (to be made...)
    public newReply: Reply = new Reply();

    // Thread ID
    public id: number;

    // Pagination stuff
    public total = 0;
    public page = 1;
    public limit = 5;

    // Router stuff
    private sub: any;

    constructor(
        private replyService: ReplyService,
        private threadService: ThreadService,
        private route: ActivatedRoute,
        private authService: AuthService,
        private router: Router,
        private notificationService: NotificationsService) { }

    ngOnInit() {
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

    /**
     * Loads the currently logged in user
     */
    getCurrentUser(): void {
        this.authService.getUser().subscribe(
            then => {
                this.user = then;
            },
            error => {
                this.router.navigate(['/error/404']);
            }
        );
    }

    /**
     * Loads the replies of the current thread
     */
    getReplies(): void {
        // Fetch the replies
        this.replyService.getReplies(this.id, this.page).subscribe(
            response => {
                this.replies = response.data;  // Update the actual replies
                this.total   = response.total; // And how many of them there are
            },
            error => {
                this.router.navigate(['/error/404']);
            }
        );
    }

    /**
     * Retrieves the thread according to the id
     * @param id the id of the thread to retrieve
     */
    getThread(id: number): void {
        this.threadService.getThread(this.id).subscribe(
            then => {
                this.thread = then;
            },
            error => {
                this.router.navigate(['/error/404']);
            }
        );
    }

    /**
     * Deletes the current thread
     */
    deleteThread(): void {
        this.threadService.deleteThread(this.id).subscribe(
            next => {
                // 200 OK
                this.router.navigate(['/forum']);
                this.notificationService.success('Success', 'Thread has been deleted');
            },
            error => {
                this.notificationService.error('Failure', 'Failed to delete the thread');
            }
        );
    }

    /**
     * Adds a reply to the current thread
     */
    addReply(): void {
        if (!this.newReply || this.newReply.body.length === 0) { return; }

        this.replyService.addReply(this.id, this.newReply).subscribe(
            next => {
                // Reply was successful, head to the latest page
                this.notificationService.success('Success', 'The reply has been added');
                this.goToPage(this.lastPage());
                this.newReply.body = ''; // Blank out the textarea
            },
            error => {
                // Something got messed up...
                this.notificationService.error('Failure', 'Failed to add the reply');
            }
        );
    }

    updateReply(id: number) {

    }

    /**
     * Deletes a specific reply
     * @param id The ID of the reply to delete
     */
    deleteReply(id: number) {
        this.replyService.deleteReply(id).subscribe(
            then => {
                this.notificationService.success('Success', 'The reply has been deleted');

                // Redirect the user to the same page --> the reply will disappear :)
                setTimeout(() => this.goToPage(this.page), 1000);
            },
            error => {
                this.notificationService.error('Failure', 'Failed to delete the reply');
            }
        );
    }

    // Pagination stuff
    lastPage(): number {
        return Math.ceil(this.total / this.limit);
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
