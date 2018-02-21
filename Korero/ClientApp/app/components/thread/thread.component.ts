import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ThreadService } from '../../services/thread.service';
import { ReplyService } from '../../services/reply.service';
import { AuthService } from '../../services/auth.service';

import { Thread } from '../../models/thread';
import { Reply } from '../../models/reply';
import { User } from '../../models/user';

@Component({
  selector: 'app-thread',
  templateUrl: './thread.component.html',
  styleUrls: ['./thread.component.css']
})
export class ThreadComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
