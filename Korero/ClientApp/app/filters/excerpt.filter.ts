import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'excerpt',
    pure: false
})
export class ExcerptFilter implements PipeTransform {
    transform(text: String, length: any): any {
        if (!text || !length)
            return text;

        return (text.length > length) ? text.substr(0, length) + '...' : text;
    }
}