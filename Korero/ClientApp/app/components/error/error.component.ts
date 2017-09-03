import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-error',
    templateUrl: './error.component.html',
})
export class ErrorComponent implements OnInit, OnDestroy {
    public id: number;
    public message: string;
    private sub: any;

    constructor(
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id'];
        });

        switch (this.id) {
            case 404:
                this.message = 'The resource you are looking for cannot be found.';
            break;
            default:
                this.message = 'An unknown error has occured.';
            break;
        }
    }

    ngOnDestroy(): void {
        this.sub.unsubscribe();
    }
}
