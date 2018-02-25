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

    public newTag: Tag = new Tag();

    public selectedTag: Tag;
    public showNewTagProps = false;

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
     * Deletes the currently selected tag.
     */
    deleteCurrentTag(): void {
        this.tagService.deleteTag(this.selectedTag).subscribe(
            then => {
                this.notificationService.success('Success', 'Tag deleted!');

                setTimeout(() => this.router.navigate(['/newthread']), 1000);
            },
            error => {
                this.notificationService.error('Failure', 'Tag not deleted!');
            }
        );
    }

    /**
     * Adds a new tag described by its color and label
     */
    addNewTag(): void {
        // Check if we have stuff set
        if (!this.newTag || !this.newTag.label || !this.newTag.color) return;

        this.tagService.createTag(this.newTag).subscribe(
            then => {
                // Heyy
                this.notificationService.success('Success', 'Tag has been successfully added');

                // Close the newtag creation dialog
                this.showNewTagProps = false;

                // Push it into the array, since we know it's been added and select it
                this.tags.push(then);
                this.selectedTag = this.tags[this.tags.length - 1];
            },
            error => {
                this.notificationService.error('Failure', 'Failed to add the new tag');
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
