import { Component } from '@angular/core';

// Imports needed for the dynamic loading bar
import {
    Router,
    Event as RouterEvent,
    NavigationStart,
    NavigationEnd,
    NavigationCancel,
    NavigationError
} from '@angular/router'

import { SlimLoadingBarService } from 'ng2-slim-loading-bar';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {

    public simpleNotificationsOptions = {
        timeOut: 5000
    }

    constructor(private router: Router, private slimLoadingBarService: SlimLoadingBarService) {
        router.events.subscribe((event: RouterEvent) => {
            // Hook router's navigation
            this.navigationInterceptor(event)
        })
    }

    navigationInterceptor(event: RouterEvent): void {
        switch (event.constructor) {
            case NavigationStart:
                this.slimLoadingBarService.start();
            break;
            case NavigationEnd:
            case NavigationCancel:
            case NavigationError:
                this.slimLoadingBarService.complete();
            break;
        }
    }
}
