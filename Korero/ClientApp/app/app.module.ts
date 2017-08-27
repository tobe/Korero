import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import {
    LocationStrategy,
    HashLocationStrategy
} from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

// Own components
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { ForumComponent } from './components/forum/forum.component';
import { PaginationComponent } from './components/pagination/pagination.component';

// Own services
import { ThreadService } from "./services/thread.service";

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        NavigationComponent,
        ForumComponent,
        PaginationComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'forum', component: ForumComponent },
            { path: 'home', component: HomeComponent },
            { path: '**', redirectTo: 'home' } // --> ^
        ])
    ],
    providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
    bootstrap: [AppComponent]
})
export class AppModule { }
