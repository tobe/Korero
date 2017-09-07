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
    id: any;
    /* https://stackoverflow.com/questions/35763730/difference-between-constructor-and-ngoninit */

    public threads: ThreadData; // API return
    public total = 0;
    public page = 1;
    public limit = 4; // This needs to be synced with Korero.Repositories.ThreadRepository.cs!

    // Filter
    tags: Tag[];
    showAll = true;

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

            this.filterTags();
        }).catch(() => this.router.navigate(['/error/404']));
    }

    getTags(): void {
        this.tagService.getTags().then(tags => {
            this.tags = tags;
        }).catch(() => this.router.navigate(['/error/404']));
    }

    tagFilter(tag: Tag, index: number, array: any): boolean {
        // Ignore intellisense here, "this" refers to "thread.tag", check filterTags below.
        return (tag.checked === true && tag.id === this.id);
    }

    clickTag(tag: Tag): void {
        tag.checked = !tag.checked;

        // If there is at least 1 tag checked, then we never want to show all.
        this.showAll = true;
        for (const tag of this.tags) {
            if (tag.checked === true) { this.showAll = false; break; }
        }

        this.filterTags();
    }
    filterTags(): void {
        for (const thread of this.threads.data) {
            if (!this.showAll) { // If we are not showing all threads, filter them
                thread.show = this.tags.find(this.tagFilter, thread.tag) ? true : false;
            } else { // Show all.
                thread.show = true;
            }
        }
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
