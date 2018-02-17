import './polyfills';
import './styles.css';
import './vendors';

import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';

// TODO: Enable for production here one day lol
// import { environment } from './environments/environment';
/*if (environment.production) {
  enableProdMode();
}*/

platformBrowserDynamic().bootstrapModule(AppModule);