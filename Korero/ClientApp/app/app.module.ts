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

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';

import { ThreadService } from './services/thread.service';

import { ExcerptFilter } from './filters/excerpt.filter';


@NgModule({
  declarations: [
      AppComponent,
      HomeComponent,
      ExcerptFilter
  ],
  imports: [
      CommonModule,
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      RouterModule.forRoot([
          { path: '', redirectTo: 'home', pathMatch: 'full' },
          { path: '**', redirectTo: 'home' }, // --> ^
          { path: 'home', component: HomeComponent }
      ])
  ],
  providers: [{ provide: LocationStrategy, useClass: HashLocationStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule { }
