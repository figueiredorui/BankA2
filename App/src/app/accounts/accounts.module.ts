import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';


import { BootstrapModalModule } from 'ng2-bootstrap-modal';

import { AccountService } from '../services/account.service';

import { SummaryComponent } from './summary/summary.component';
import { AddAccountComponent } from './add-account/add-account.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    BootstrapModalModule
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
