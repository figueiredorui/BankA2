import { Component, OnInit, Input } from '@angular/core';

import { AccountService } from '../../services/account.service';

@Component({
  selector: 'dashboard-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit {

  @Input()
  set accountID(accountID: number) {
    this.LoadAccount(accountID);
  }

  public errorMsg: string;
  public Account: any;

  constructor(
    private accountService: AccountService) { }

  ngOnInit() {
  }

  private LoadAccount(accountID) {
    this.accountService.getAccount(accountID)
      .subscribe(data => {
        this.Account = data;
      }, err => {
        this.errorMsg = err;
      });
  }

}
