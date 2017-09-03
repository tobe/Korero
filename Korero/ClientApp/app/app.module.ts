import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
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
import { SimpleNotificationsModule } from 'angular2-notifications';

// Own components
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { ForumComponent } from './components/forum/forum.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { AvatarComponent } from './components/avatar/avatar.component';
import { ThreadComponent } from './components/thread/thread.component';
import { ErrorComponent } from './components/error/error.component';
import { SimpleMDEComponent } from './components/simplemde/simplemde.component';

// Own services
import { ThreadService } from './services/thread.service';
import { ReplyService } from './services/reply.service';
import { AuthService } from './services/auth.service';
import { TagService } from './services/tag.service';

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
        ErrorComponent,
        ExcerptFilter,
        SimpleMDEComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        HttpModule,
        FormsModule,
        SlimLoadingBarModule.forRoot(), // Singleton
        BrowserAnimationsModule,
        SimpleNotificationsModule.forRoot(),
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'forum', component: ForumComponent },
            { path: 'thread/:id', component: ThreadComponent },
            { path: 'error/:id', component: ErrorComponent },
            { path: 'home', component: HomeComponent },
            { path: '**', redirectTo: 'home' } // --> ^
        ])
    ],
    providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
    bootstrap: [AppComponent]
})
export class AppModule { }
