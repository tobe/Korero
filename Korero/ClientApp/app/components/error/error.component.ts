import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

@Component({
    selector: 'app-error',
    templateUrl: './thread.component.html',
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
                this.message = "The resource you are looking for cannot be found.";
            break;
            case 
        }
    }

    ngOnDestroy(): void {
        this.sub.unsubscribe();
    }
}
