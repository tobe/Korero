import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ThreadService } from '../../services/thread.service';
import { TagService } from '../../services/tag.service';

import { ThreadData } from '../../models/thread';

import { Tag } from '../../models/tag';

@Component({
    selector: 'app-forum',
    templateUrl: './forum.component.html',
    styleUrls: ['./forum.component.css'],
    providers: [ThreadService, TagService]
})
export class ForumComponent implements OnInit {
    /* https://stackoverflow.com/questions/35763730/difference-between-constructor-and-ngoninit */

    public threads: ThreadData; // API return
    public total = 0;
    public page = 1;
    public limit = 4; // This needs to be synced with Korero.Repositories.ThreadRepository.cs!

    // Filter
    tags: Tag[];   

    // Same as private threadservice;  threadservice = ThreadService... Some neat DI
    constructor(
        private threadService: ThreadService,
        private tagService: TagService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.getTags();
        this.getThreads();
    }

    getThreads(): void {
        this.threadService.getThreads(this.page).then(threads => {
            this.threads = threads;
            this.total = threads.total;
        }).catch(() => this.router.navigate(['/error/404']));

        this.filterTags();
    }

    getTags(): void {
        this.tagService.getTags().then(tags => {
            this.tags = tags;
        }).catch(() => this.router.navigate(['/error/404']));
    }

    clickTag(): void {
        // set checked = false
        // call filterTags
    }
    filterTags(): void {
        /*
        Loop through this.threads
        check if a thread's tag is checked, if so, set thread.filter = true
        */
    }

    goToPage(n: number): void {
        this.page = n;
        this.getThreads();
    }

    onNext(): void {
        this.page++;
        this.getThreads();
    }

    onPrev(): void {
        this.page--;
        this.getThreads();
    }
}
