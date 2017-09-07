import { Component, Input, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { markdown } from 'markdown';

@Component({
    selector: 'app-markdown',
    template: `<div [innerHtml]="html" #root></div>`
})
export class MarkdownComponent {
    @ViewChild('root') root: ElementRef;
    html: string;

    @Input()
    set data(value: string) {
        this.html = markdown.toHTML(value);
    }

    constructor() { }
}