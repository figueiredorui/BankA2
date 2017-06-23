import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { BootstrapModalModule } from 'ng2-bootstrap-modal';
//import { ChartsModule } from 'ng2-charts';
import { NgxPaginationModule } from 'ngx-pagination';
import { Ng2CompleterModule } from "ng2-completer";
import { BusyModule, BusyConfig, BUSY_CONFIG_DEFAULTS } from 'angular2-busy';

import { CoreModule } from "../core/core.module"; 
import { DashboardComponent } from './dashboard.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { ChartsComponent } from './charts/charts.component';
import { ImportComponent } from './import/import.component';
import { FilesComponent } from './files/files.component';
import { TagsComponent } from './tags/tags.component';
import { HeaderComponent } from './header/header.component';
import { ConfirmationDialogComponent } from '../core/confirmation-dialog/confirmation-dialog.component';

import { TransactionsService } from '../services/transactions.service';
import { FileService } from '../services/file.service';
import { ColorsService } from '../core/services/colors.service';


const busyConfig: BusyConfig = {
    message: '',
    delay: BUSY_CONFIG_DEFAULTS.delay,
    template: BUSY_CONFIG_DEFAULTS.template,
    minDuration: BUSY_CONFIG_DEFAULTS.minDuration,
    backdrop: false,
    wrapperClass: BUSY_CONFIG_DEFAULTS.wrapperClass
};


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
  //  ChartsModule,
    RouterModule,
    BootstrapModalModule,
    NgxPaginationModule,
    Ng2CompleterModule,
    BusyModule.forRoot(busyConfig),
    CoreModule
  ],
  exports: [
  ],
  entryComponents: [
    ImportComponent,
    ConfirmationDialogComponent
  ],
  providers: [TransactionsService, FileService, ColorsService],
  declarations: [TransactionsComponent, DashboardComponent , ChartsComponent, ImportComponent, FilesComponent, HeaderComponent, TagsComponent]
})
export class DashboardModule { }
