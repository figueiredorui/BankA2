import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';

import { NgLoadingBarModule } from 'ng-loading-bar';
import { ToastModule } from 'ng2-toastr/ng2-toastr';

import { DashboardModule } from './dashboard/dashboard.module';
import { AccountsModule } from './accounts/accounts.module';
import { TagsModule } from './tags/tags.module';
import { CoreModule } from './core/core.module';

import { RequestOptionsProvider } from './core/services/http-api.service';

import { AppComponent } from './app.component';

/*
load(
   // supplemental data
   require('cldr-data/supplemental/likelySubtags.json'),
   require('cldr-data/supplemental/weekData.json'),
   require('cldr-data/supplemental/currencyData.json'),

   // locale data
   require('../../node_modules/cldr-data/main/de/numbers.json'),
   require('cldr-data/main/de/currencies.json'),
   require('cldr-data/main/de/ca-gregorian.json'),
   require('cldr-data/main/de/timeZoneNames.json')
);
*/

@NgModule({
  declarations: [
    AppComponent,


  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NgLoadingBarModule.forRoot(),
    ToastModule.forRoot(),
    DashboardModule,
    AccountsModule,
    TagsModule,
    CoreModule
  ],
  providers: [ RequestOptionsProvider, { provide: LOCALE_ID, useValue: 'en-GB' }],
  bootstrap: [AppComponent]
})
export class AppModule { }

