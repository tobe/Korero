import { Component, OnInit } from '@angular/core';

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

    constructor(
        private replyService: ReplyService,
        private threadService: ThreadService,
        private tagService: TagService,
        private notificationService: NotificationsService
    ) {}

    ngOnInit() {
        // Fetch all the tags
        this.getTags();
    }

    getTags(): void {
        this.tagService.getTags().then(tags => this.tags = tags);
    }

}
