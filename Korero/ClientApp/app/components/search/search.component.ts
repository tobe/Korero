import { Component, OnInit, Input, OnChanges, SimpleChanges, ChangeDetectorRef, NgZone } from '@angular/core';
import { Thread } from '../../models/thread';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnChanges {
    @Input() threads: Thread[];
    criteria: string;

    constructor(private changeDetect: ChangeDetectorRef) { }

    ngOnChanges(changes: SimpleChanges) {
        if (!this.threads || !this.criteria) return;

        // Delay the execution for 1 tick, wait before it's rendered to change the model
        // Otherwise... it doesn't simply work.
        setTimeout(() => this.doSearch(), 0);

        this.changeDetect.detectChanges();
    }

    doSearch(): void {
        if (this.criteria.length == 0) return;
        this.criteria = this.criteria.toLowerCase();

        // Just .filter...
        const filteredThreads = this.threads.filter(t => {
            return t.title.toLowerCase().indexOf(this.criteria) > -1 ||
                   t.replies[0].body.indexOf(this.criteria) > -1
        });

        this.threads.forEach(t => {
            t.show = (filteredThreads.includes(t)) ? true : false;
            this.changeDetect.markForCheck();
        });
    }
}
