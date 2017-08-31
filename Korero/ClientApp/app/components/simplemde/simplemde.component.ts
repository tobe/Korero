// https://github.com/sparksuite/simplemde-markdown-editor/issues/367
import { AfterViewInit, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';

const _SimpleMDE: any = require('simplemde');

@Component({
    selector: 'app-simplemde',
    template: `<textarea #simplemde></textarea>`
})
export class SimpleMDE {
    @Output() modelChange = new EventEmitter<string>();
    @ViewChild('simplemde') textarea: ElementRef;

    constructor(private elementRef: ElementRef) { }

    ngAfterViewInit() {
        const modelChange = this.modelChange;
        const mde = new _SimpleMDE({
            element: this.textarea.nativeElement,
            forceSync: true,
            status: false
        });

        mde.codemirror.on('change', function () {
            const value = mde.codemirror.getValue();
            modelChange.emit(value);
        });
    }
}