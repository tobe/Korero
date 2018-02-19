import { Component, Input } from '@angular/core';

@Component({
    selector: `app-avatar`,
    templateUrl: './avatar.component.html'
})

export class AvatarComponent {
    @Input() letter: string; // Grab a letter from the user
}
