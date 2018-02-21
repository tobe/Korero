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

// Components
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AvatarComponent } from './components/avatar/avatar.component';
import { PaginationComponent } from './components/pagination/pagination.component';

// Services
import { ThreadService } from './services/thread.service';

// Filters
import { ExcerptFilter } from './filters/excerpt.filter';


@NgModule({
    declarations: [
      ExcerptFilter,
      AppComponent,
      HomeComponent,
      AvatarComponent,
      PaginationComponent
  ],
  imports: [
      CommonModule,
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      RouterModule.forRoot([
          { path: '', redirectTo: 'home', pathMatch: 'full' },
          { path: '**', redirectTo: 'home' },
          { path: 'home', component: HomeComponent }
      ]),
      NgProgressModule.forRoot(),
      NgProgressHttpModule,
      NgProgressModule.forRoot(),
      NgProgressRouterModule
  ],
  providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule { }
