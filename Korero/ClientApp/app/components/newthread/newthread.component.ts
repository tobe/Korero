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

    getTags(): void {
        this.tagService.getTags().then(tags => {
            this.tags = tags;

            this.selectedTag = tags[0];
        });
    }

    addThread(): void {
        this.threadService.createThread(this.thread.title, this.selectedTag.id)
            .then(thread => {
                console.log("new thread id: " + thread.id);
                this.replyService.addReply(thread.id, this.reply)
                    .then(() => {
                        this.notificationService.success('Success', 'Thread created');
                        this.router.navigate(['/thread/' + thread.id]);
                    })
                    .catch(() => {
                        this.router.navigate(['/error/400']);
                    });
            }).catch(() => this.router.navigate(['/error/400']));
    }
}
