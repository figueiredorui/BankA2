import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { BootstrapModalModule } from 'ng2-bootstrap-modal';

import { CoreModule } from "../core/core.module"; 

import { SummaryComponent } from './summary/summary.component';
import { AddAccountComponent } from './add-account/add-account.component';

import { AccountService } from '../services/account.service';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    BootstrapModalModule,
    CoreModule
  ],
  exports: [
    SummaryComponent
  ],
  entryComponents: [
    AddAccountComponent
  ],
  providers: [AccountService],
  declarations: [SummaryComponent, AddAccountComponent]
})
export class AccountsModule { }
