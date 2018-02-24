import { Component, OnInit, Input } from '@angular/core';
import { Thread } from '../../models/thread';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
    @Input() threads: Thread[];

    constructor() { }

    ngOnInit() {

    }

    doSearch(value: string): void {
        if (value.length == 0) return;
        value = value.toLowerCase();

        // Just .filter...
        const newThreads = this.threads.filter(t => {
            return t.title.toLowerCase().indexOf(value) > -1 ||
                   t.replies[0].body.indexOf(value) > -1
        });

        this.threads = newThreads;

        //console.log(x);
    }
}
