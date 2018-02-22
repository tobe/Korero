import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ThreadService } from '../../services/thread.service';
import { ReplyService } from '../../services/reply.service';
import { TagService } from '../../services/tag.service';

import { Thread } from '../../models/thread';
import { Reply } from '../../models/reply';
import { Tag } from '../../models/tag';

import { NotificationsService } from 'angular2-notifications';

@Component({
    selector: 'app-newthread',
    templateUrl: './newthread.component.html',
    styleUrls: ['./newthread.component.css'],
    providers: [ThreadService, ReplyService, TagService]
})
export class NewThreadComponent implements OnInit {
    public reply: Reply = new Reply();
    public thread: Thread = new Thread();
    public tags: Tag[];

    public selectedTag: Tag;

    constructor(
        private replyService: ReplyService,
        private threadService: ThreadService,
        private tagService: TagService,
        private router: Router,
        private notificationService: NotificationsService
    ) {}

    ngOnInit() {
        // Fetch all the tags
        this.getTags();
    }

    /**
     * Returns all the available tags
     */
    getTags(): void {
        this.tagService.getTags().subscribe(
            then => {
                this.tags = then;
                this.selectedTag = then[0];
            },
            error => {
                this.router.navigate(['/error/500']);
            }
        );
    }

    /**
     * Adds a new thread
     */
    addThread(): void {
        // Check whether the title and the reply are not empty
        if (this.thread.title.length === 0 || this.reply.body.length === 0) { return; }

        // Assign the selected tag to the new thread object
        this.thread.tag = this.selectedTag;

        // Alles gut
        this.threadService.createThread(this.thread).subscribe(
            then => {
                this.replyService.addReply(then.id, this.reply).subscribe(
                    ok => {
                        this.notificationService.success('Success', 'Thread created!');
                        this.router.navigate(['thread', then.id]);
                    },
                    fail => {
                        this.notificationService.error('Failure', 'Failed to create the thread!');
                        this.router.navigate(['/error/400']);
                    }
                );
            },
            error => this.router.navigate(['/error/400'])
        );
    }
}
