import { Component } from '@angular/core';

import { ThreadService } from '../../services/thread.service';
import { ReplyService } from '../../services/reply.service';

import { Thread } from '../../models/thread';
import { Reply } from '../../models/reply';

import { NotificationsService } from 'angular2-notifications';

@Component({
    selector: 'app-newthread',
    templateUrl: './newthread.component.html',
    providers: [ThreadService, ReplyService]
})
export class NewThreadComponent {
    public reply: Reply = new Reply();
    public thread: Thread = new Thread();

    constructor(
        private replyService: ReplyService,
        private threadService: ThreadService,
        private notificationService: NotificationsService
    ) {}



}