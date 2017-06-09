import { Component, OnInit } from '@angular/core';
import 'rxjs/add/operator/catch';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

import { AccountService } from '../../services/account.service';
import { Account } from '../accounts.types';

export interface AddAccountParam {
  accountID: number;
}
@Component({
  selector: 'accounts-add-account',
  templateUrl: 'add-account.component.html',
})
export class AddAccountComponent extends DialogComponent<AddAccountParam, boolean> implements AddAccountParam, OnInit {
  public accountID: number;
  public errorMsg: string = null;
  public account: Account;

  constructor(dialogService: DialogService, private accountService: AccountService) {
    super(dialogService);
  }

  public ngOnInit() {
    this.loadAccount();
  }

  private loadAccount() {
    if (this.accountID > 0) {
      this.accountService.getAccount(this.accountID)
        .subscribe(data => this.account = data,
        err => this.errorMsg = err);
    } else {
      this.account = {
        AccountID: 0,
        Description: '',
        ImportCsvDefinition:'',
        Closed: false
      };
    }
  }

  public cancel() {
    this.result = false;
    this.close();
  }

  public saveAccount() {
    this.accountService.saveAccount(this.account)
      .subscribe(data => {
        this.account = data;
        this.result = true;
        this.close();
      }, err => this.errorMsg = err);
  }

  public deleteAccount() {
    this.accountService.deleteAccount(this.account.AccountID)
      .subscribe(data => {
        this.account = data;
        this.result = true;
        this.close();
      }, err => this.errorMsg = err);
  }
}
