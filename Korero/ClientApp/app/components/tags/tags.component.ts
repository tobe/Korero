import { Component, Input, OnInit, OnChanges } from '@angular/core';
import { TagService } from '../../services/tag.service';

import { Tag } from '../../models/tag';
import { Thread } from '../../models/thread';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.css'],
  providers: [TagService]
})
export class TagsComponent implements OnInit, OnChanges {
    public tags: Tag[];
    @Input() threads: Thread[];

    constructor(
        private tagService: TagService) {}

    ngOnChanges() {
        if (!this.tags) return;

        this.filterTags();
    }

    ngOnInit() {
        this.getTags();
    }

    /**
     * Loads all the tags.
     */
    getTags(): void {
        this.tagService.getTags().subscribe(then => this.tags = then);
    }

    /**
     * Handles clicks on tags
     * @param tag The tag that has been clicked on
     */
    clickTag(tag: Tag): void {
        tag.checked = !tag.checked;

        this.filterTags();
    }

    /**
     * Filters out selected tags
     */
    filterTags(): void {
        // Find out how many tags are unchecked
        let uncheckedTags = 0;
        for (let tag of this.tags) {
            if (!tag.checked) uncheckedTags++;
        }

        // Are all tags unchecked?
        let allUnchecked = false;
        if (uncheckedTags == this.tags.length) allUnchecked = true;

        // Find the threads with the clicked tag and update them accordingly
        for (const thread of this.threads) {
            // If all are unchecked... show all threads!
            if (allUnchecked) {
                thread.show = true;
                continue;
            }

            thread.show = this.tags.find(t => (t.checked === true && t.id === thread.tag.id)) ? true : false;
        }
    }

}
