import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';

import { ThreadService } from '../../services/thread.service';

import { Thread } from '../../models/thread';
import { Observable } from 'rxjs/Observable';


@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    providers: [ThreadService]
})
export class HomeComponent implements OnInit {
    // Will hold the results of the API call
    public threads: Thread[];

    // Pagination stuff
    public total = 0;
    public page  = 1;
    public limit = 4; // This needs to be synced with Korero.Repositories.ThreadRepository.cs!

    constructor(
        private threadService: ThreadService,
        private router: RouterModule
    ) { }

    ngOnInit(): void {
        this.getThreads();
    }

    getThreads(): void {
        this.threadService.getThreads(this.page).subscribe(resp => {
            this.threads = resp.data;
            this.total = resp.total;

            for (let thread of this.threads) thread.show = true;
        });
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
