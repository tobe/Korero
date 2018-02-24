import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import {
    LocationStrategy,
    HashLocationStrategy
} from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// Vendor stuff
import { NgProgressModule } from '@ngx-progressbar/core';
import { NgProgressHttpModule } from '@ngx-progressbar/http';
import { NgProgressRouterModule } from '@ngx-progressbar/router';
import { SimpleNotificationsModule } from 'angular2-notifications';

// Components
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AvatarComponent } from './components/avatar/avatar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { ThreadComponent } from './components/thread/thread.component';
import { ErrorComponent } from './components/error/error.component';
import { SimpleMDEComponent } from './components/simplemde/simplemde.component';
import { NewThreadComponent } from './components/newthread/newthread.component';
import { TagsComponent } from './components/tags/tags.component';
import { SearchComponent } from './components/search/search.component';

// Services
import { ThreadService } from './services/thread.service';
import { ReplyService } from './services/reply.service';
import { AuthService } from './services/auth.service';

// Filters
import { ExcerptFilter } from './filters/excerpt.filter';
import { MarkdownToHtmlModule } from 'markdown-to-html-pipe';


@NgModule({
    declarations: [
      ExcerptFilter,
      AppComponent,
      HomeComponent,
      AvatarComponent,
      PaginationComponent,
      ThreadComponent,
      ErrorComponent,
      SimpleMDEComponent,
      NewThreadComponent,
      TagsComponent,
      SearchComponent
  ],
  imports: [
      CommonModule,
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      RouterModule.forRoot([
          { path: 'error/:id', component: ErrorComponent },
          { path: 'thread/:id', component: ThreadComponent },
          { path: 'newthread', component: NewThreadComponent },
          { path: 'home', component: HomeComponent },
          { path: '', redirectTo: 'home', pathMatch: 'full' },
          { path: '**', component: ErrorComponent }
      ], { enableTracing: false }),
      SimpleNotificationsModule.forRoot(),
      NgProgressModule.forRoot(),
      NgProgressHttpModule,
      NgProgressRouterModule,
      MarkdownToHtmlModule
  ],
  providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule { }
