import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ThreadService } from '../../services/thread.service';
import { AuthService } from '../../services/auth.service';

import { Thread } from '../../models/thread';
import { Reply } from '../../models/reply';
import { User } from '../../models/user';

import { NotificationsService } from 'angular2-notifications';

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

    // Inline edit. A bool array identifying which of the replies are in edit state.
    public isEditableArray: boolean[] = [false];

    // Some temporary variables to store various replies
    public newReply: Reply = new Reply();
    public oldReplies: Reply[];

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
        private authService: AuthService,
        private router: Router,
        private notificationService: NotificationsService
    ) {
        for (let i = 0; i < 64; i++) {
            this.isEditableArray[i] = false;
        }
    }

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

    isEditable(index: number): boolean {
        if (typeof this.isEditableArray[index] === 'undefined' ||
            this.isEditableArray[index] == false)
                return false;

        return true;
    }
    setEditable(index: number, value: boolean): void {
        if (typeof this.isEditableArray[index] !== 'undefined')
            this.isEditableArray[index] = value;

        // If the value is false -> user canceled the editing, swap the original reply
        if (value == false) {
            console.log(this.replies[index], this.oldReplies[index]);
            this.replies[index] = this.oldReplies[index];
        }
    }

    // Returns the current user
    getCurrentUser(): void {
        this.authService.getUser().then(user => this.user = user);
    }

    // Returns all the thread's replies
    getReplies(): void {
        this.threadService.getReplies(this.id, this.page).then(replies => {
            this.replies    = replies.data;
            // Make a deep copy of the replies
            this.oldReplies = ThreadComponent.deepCopy(replies.data);
            this.total      = replies.total;
        }).catch(() => this.router.navigate(['/error/404']));

        for (let i = 0; i < this.total; i++) {
            this.isEditableArray[i] = false;
        }
    }

    // Returns information about a specified thread
    getThread(id: number): void {
        this.threadService.getThread(this.id)
            .then(thread => this.thread = thread)
            .catch(()    => this.router.navigate(['/error/404']));
    }

    // Deletes a thread
    deleteThread(): void {
        this.threadService.deleteThread(this.id)
            .then(() => {
                this.router.navigate(['/forum']);
                this.notificationService.success("Thread deleted");
            })
            .catch(() => {
                this.notificationService.error("Thread NOT deleted");
            });
    }

    // Adds a reply
    addReply(): void {
        if (!this.newReply.body || this.newReply.body.length === 0) return;

        this.threadService.addReply(this.id, this.newReply)
            .then(() => {
                // Reply was successful, head to the latest page
                this.notificationService.success("Reply added");
                this.goToPage(this.lastPage());
                this.newReply.body = ""; // Blank out the textarea
            });
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
        var newObj = oldObj;
        if (oldObj && typeof oldObj === "object") {
            newObj = Object.prototype.toString.call(oldObj) === "[object Array]" ? [] : {};
            for (var i in oldObj) {
                newObj[i] = this.deepCopy(oldObj[i]);
            }
        }
        return newObj;
    }
}
