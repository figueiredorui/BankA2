import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { ChartsModule } from 'ng2-charts';
import { NgxPaginationModule } from 'ngx-pagination';
import { Ng2CompleterModule } from "ng2-completer";
import { Daterangepicker } from 'ng2-daterangepicker';
import { CoreModule } from "../core/core.module";
import {BusyModule,BusyConfig} from 'angular2-busy';

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


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ChartsModule,
    RouterModule,
    BootstrapModalModule,
    NgxPaginationModule,
    Ng2CompleterModule,
    Daterangepicker,
    BusyModule.forRoot(
        	new BusyConfig({
            	message: '',
                backdrop: false,
            })
        ),
    CoreModule
  ],
  exports: [
  ],
  entryComponents: [
    ImportComponent,
    ConfirmationDialogComponent
  ],
  providers: [TransactionsService, FileService, ColorsService],
  declarations: [TransactionsComponent, ChartsComponent, ImportComponent, FilesComponent, HeaderComponent, TagsComponent]
})
export class DashboardModule { }
