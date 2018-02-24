import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
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

    // Future edited replies (for inline edit)
    public editedReplies: Reply[] = [];

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
        private notificationService: NotificationsService,
        private ref: ChangeDetectorRef) { }

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
                this.total = response.total; // And how many of them there are

                console.log(this.replies);

                // Wipe and allocate enough memory in the editedReplies array
                this.editedReplies = [];
                for (let i = 0; i < response.total; i++) {
                    // Clone the reply and add it into the newly created array
                    let clonedReply = ThreadComponent.deepCopy(this.replies[i]);
                    this.editedReplies.push(clonedReply);
                }
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
                this.router.navigate(['/']);
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

                this.newReply.body = ''; // Blank out the textarea
                // Bump the count so the lastPage correctly wraps around
                this.total++;
                this.goToPage(this.lastPage());
            },
            error => {
                // Something got messed up...
                this.notificationService.error('Failure', 'Failed to add the reply');
            }
        );
    }

    /**
     * Toggles the editing of a response, inline.
     * @param index The index of the reply (response) to edit or stop editing
     * @param bool True for editable, false for not editable
     */
    setEditable(index: number, bool: boolean): void {
        const reply = this.replies[index];

        console.log(reply, this.user);

        // We can only edit our own replies
        if (reply.author.userName != this.user.userName) return;

        // Set the boolean
        reply.editing = bool;
    }

    /**
     * Updates a reply by the index
     * @param index the index of the reply to update
     */
    updateReply(index: number): void {
        // Get the edited reply
        const reply = this.editedReplies[index];
        if (!reply) return;

        // Update
        this.replyService.updateReply(reply).subscribe(
            then => {
                this.notificationService.success('Success', 'The reply has been updated');
            },
            error => {
                this.notificationService.error('Failure', 'Failed to update the reply');
            }
        );

        // Update the replies.
        setTimeout(() => this.goToPage(this.page), 1000);
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

    /**
     * Returns a deep copy of the object.
     * Not gonna rely on lodash or some shit to use this, since Ang2 doesn't
     * support this out of the box like ang1 does.
     * @url https://stackoverflow.com/a/38722431
     */
    public static deepCopy(oldObj: any) {
        let newObj = oldObj;
        if (oldObj && typeof oldObj === 'object') {
            newObj = Object.prototype.toString.call(oldObj) === '[object Array]' ? [] : {};
            for (const i in oldObj) {
                newObj[i] = this.deepCopy(oldObj[i]);
            }
        }
        return newObj;
    }
}
