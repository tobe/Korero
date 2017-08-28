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

// Vendor components
import { SlimLoadingBarModule } from 'ng2-slim-loading-bar';

// Own components
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { ForumComponent } from './components/forum/forum.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { AvatarComponent } from './components/avatar/avatar.component';
import { ThreadComponent } from './components/thread/thread.component';

// Own services
import { ThreadService } from './services/thread.service';

// Own filters
import { ExcerptFilter } from './filters/excerpt.filter';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        NavigationComponent,
        ForumComponent,
        PaginationComponent,
        AvatarComponent,
        ThreadComponent,
        ExcerptFilter
    ],
    imports: [
        CommonModule,
        BrowserModule,
        HttpModule,
        FormsModule,
        SlimLoadingBarModule.forRoot(), // Singleton
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'forum', component: ForumComponent },
            { path: 'thread/:id', component: ThreadComponent },
            { path: 'home', component: HomeComponent },
            { path: '**', redirectTo: 'home' } // --> ^
        ])
    ],
    providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
    bootstrap: [AppComponent]
})
export class AppModule { }
