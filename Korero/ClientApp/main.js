"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("./polyfills");
require("./styles.css");
require("./vendors");
var platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
var app_module_1 = require("./app/app.module");
// TODO: Enable for production here one day lol
// import { environment } from './environments/environment';
/*if (environment.production) {
  enableProdMode();
}*/
platform_browser_dynamic_1.platformBrowserDynamic().bootstrapModule(app_module_1.AppModule);
//# sourceMappingURL=main.js.map