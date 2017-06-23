import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { DialogService } from 'ng2-bootstrap-modal';

import { TransactionsService } from '../../services/transactions.service';
import { TagService } from '../../services/tag.service';
import { Transaction, TransactionResult, TransactionSearch } from '../dashboard.types';
import { ImportComponent } from '../import/import.component';
import { TagDialogComponent } from '../../tags/tag-dialog/tag-dialog.component'

@Component({
  moduleId: module.id,
  selector: 'dashboard-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.scss']
})
export class TransactionsComponent implements OnInit {

  public isBusy: any;
  public errorMsg: string;
  public transactions: Transaction[];
  public accountTags: string[];

  public itemsPerPage: number;
  public page: number;
  public total: number;
  public loading: boolean;
  public search: string = '';

  public tags: string[] = [];

  public accountID: number;

  @Input()
  set showAccountID(value: number) {
    this.accountID = value;

    this.Search()
  }

  constructor(
    private transactionsService: TransactionsService,
    private tagsService: TagService,

    private dialogService: DialogService,
  ) { }

  public ngOnInit() {

    this.LoadTagsLookup();
  }

  public onTagChanged(transaction: Transaction) {

    this.transactionsService.updateTag(transaction)
      .subscribe(data => data,
      err => this.errorMsg = err);
  }

  public Search() {
    const search: TransactionSearch = { Page: 1, Query: this.search };
    this.LoadTransactions(this.accountID, search);
  }

  public getPage(page: number) {
    this.loading = true;
    const search: TransactionSearch = { Page: page, Query: this.search };
    this.LoadTransactions(this.accountID, search);
  }

  public ImportFile() {
    const disposable = this.dialogService.addDialog(ImportComponent, { accountID: this.accountID }, { backdropColor: 'rgba(0, 0, 0, 0.4)' })
      .subscribe((isConfirmed) => {
        this.Search();
      });
  }

  public addTag(transaction: Transaction) {
    const disposable = this.dialogService.addDialog(TagDialogComponent, { tagID: 0, transaction: transaction }, { backdropColor: 'rgba(0, 0, 0, 0.4)' })
      .subscribe((tag) => {
        if (tag) {
          transaction.Tag = tag.Tag;
          this.transactionsService.updateTag(transaction)
            .subscribe(data => {
              this.Refresh();
            },
            err => this.errorMsg = err);

        }
      });
  }

  public MarkAsTransfer(transaction: Transaction) {
    transaction.IsTransfer = true;
    this.transactionsService.markAsTransfer(transaction)
      .subscribe(data => {
        this.Refresh();
      },
      err => this.errorMsg = err);
  }

  public UnmarkAsTransfer(transaction: Transaction) {
    transaction.IsTransfer = false;
    this.transactionsService.markAsTransfer(transaction)
      .subscribe(data => {
        this.Refresh();
      },
      err => this.errorMsg = err);
  }

  private Refresh() {
    const search: TransactionSearch = { Page: this.page, Query: this.search };
    this.LoadTransactions(this.accountID, search);
  }

  private LoadTransactions(accountID: number, search: TransactionSearch) {
    this.isBusy = this.transactionsService.getTransactions(accountID, search)
      .subscribe(data => {
        this.total = data.Count;
        this.page = data.Page;
        this.itemsPerPage = data.ItemsPerPage;
        this.transactions = data.Data;
      }, err => {
        this.errorMsg = err;
      });
  }

  private LoadTagsLookup() {
    this.tagsService.getTagsLookup()
      .subscribe(data => {
        this.tags = data
      }, err => {
        this.errorMsg = err;
      });
  }
}
