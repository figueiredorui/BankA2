import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute, Params, NavigationEnd, Event } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { DialogService } from 'ng2-bootstrap-modal';

import { AccountService } from '../../services/account.service';
import { AddAccountComponent } from '../add-account/add-account.component';

@Component({
  moduleId: module.id,
  selector: 'accounts-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {

  public errorMsg: string;
  public Accounts: any[];
  public view: string;

  constructor(
    private dialogService: DialogService,
    private accountService: AccountService,
    private router: Router, private activatedRoute: ActivatedRoute) {

    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .subscribe((event: Event) => {
        if (this.router.url.indexOf('files') > 0)
          this.view = 'files';
        else
          this.view = 'dashboard';
      });

  }

  public ngOnInit() {
    this.LoadSummary();
  }

  public addAcount() {
    const disposable = this.dialogService.addDialog(AddAccountComponent, { accountID: 0 }, { backdropColor: 'rgba(0, 0, 0, 0.4)' })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.LoadSummary();
        }
      });
  }

  public editAccount(accountID) {
    const disposable = this.dialogService.addDialog(AddAccountComponent, { accountID }, { backdropColor: 'rgba(0, 0, 0, 0.4)' })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.LoadSummary();
        }
      });
  }

  private LoadSummary() {
    this.accountService.getList()
      .subscribe(data => {
        this.Accounts = data;
      }, err => {
        this.errorMsg = err;
      });
  }
}
