import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

import { TransactionsService } from '../../services/transactions.service';
import { FileService } from '../../services/file.service';
import { AccountService } from '../../services/account.service';
import { ImportCsvDefinition } from '../dashboard.types';
import { Account } from '../../accounts/accounts.types';


export interface ImportParam {
  accountID: number;
}

export interface IColumnMapping {
  ColumnId: string;
  ColumnFieldMap: string;
}

@Component({
  selector: 'dashboard-import',
  templateUrl: './import.component.html',
})
export class ImportComponent extends DialogComponent<ImportParam, boolean> implements ImportParam, OnInit {
  public accountID: number;
  public successMsg: string;
  public errorMsg: string;

  public ColumnMapping: IColumnMapping[];
  public CsvDataJson: any;

  public FieldNameLookup: string[];

  public hasHeaders: boolean;

  private account: Account;
  private input: any;

  constructor(
    dialogService: DialogService,
    private transactionService: TransactionsService,
    private accountService: AccountService,
    private fileService: FileService) {
    super(dialogService);
  }

  public ngOnInit() {

    this.CreateFieldNameLookup();
    this.loadAccount();

  }

  private loadAccount() {
    this.accountService.getAccount(this.accountID)
      .subscribe(data => this.account = data,
      err => this.errorMsg = err);
  }

  public onChange(event) {
    const self = this;
    const files = event.srcElement.files;
    if (files[0].name.includes('.csv')) {

      this.input = event.target;
      const reader = new FileReader();

      reader.onload = function () {

        self.fileService.parseFile(event.target.files[0])
          .subscribe(data => {

            self.CsvDataJson = data;
            self.ColumnMapping = [];

            const columns = Object.keys(self.CsvDataJson[0]);
            for (const i in columns) {
              const column = columns[i];

              self.ColumnMapping[i] = {
                ColumnId: column,
                ColumnFieldMap: 'None'
              };
            }

            self.ReadImportCsvDefinition();


          }, err => {
            //this.errorMsg = err;
          });
      };
      reader.readAsText(this.input.files[0]);
    }
  }

  public Import() {
    this.successMsg = null;
    this.errorMsg = null;

    const importDefinition = this.CreateImportCsvDefinition();

    this.transactionService.import(this.accountID, importDefinition, this.input.files[0])
      .subscribe(data => {
        this.successMsg = data;
        this.close();
      }, err => {
        this.errorMsg = err;
      });
  }

  public Cancel() {
    this.close();
  }

  private CreateFieldNameLookup(): void {

    this.FieldNameLookup = [];
    this.FieldNameLookup.push('None');
    this.FieldNameLookup.push('TransactionDate');
    this.FieldNameLookup.push('TransactionType');
    this.FieldNameLookup.push('Description');
    this.FieldNameLookup.push('CreditAmount');
    this.FieldNameLookup.push('DebitAmount');
    this.FieldNameLookup.push('Amount');

  }

  private ReadImportCsvDefinition() {
    if (this.account.ImportCsvDefinition) {

      let importCsvDefinition: ImportCsvDefinition = JSON.parse(this.account.ImportCsvDefinition);
      this.hasHeaders = importCsvDefinition.HasHeaders;

      if (importCsvDefinition.TransactionDate_Index > -1)
        this.ColumnMapping[importCsvDefinition.TransactionDate_Index].ColumnFieldMap = 'TransactionDate';
      if (importCsvDefinition.TransactionType_Index > -1)
        this.ColumnMapping[importCsvDefinition.TransactionType_Index].ColumnFieldMap = 'TransactionType';
      if (importCsvDefinition.Description_Index > -1)
        this.ColumnMapping[importCsvDefinition.Description_Index].ColumnFieldMap = 'Description';
      if (importCsvDefinition.CreditAmount_Index > -1)
        this.ColumnMapping[importCsvDefinition.CreditAmount_Index].ColumnFieldMap = 'CreditAmount';
      if (importCsvDefinition.DebitAmount_Index > -1)
        this.ColumnMapping[importCsvDefinition.DebitAmount_Index].ColumnFieldMap = 'DebitAmount';
      if (importCsvDefinition.Amount_Index > -1)
        this.ColumnMapping[importCsvDefinition.Amount_Index].ColumnFieldMap = 'Amount';
    }
  }

  private CreateImportCsvDefinition(): ImportCsvDefinition {

    const importDefinition: ImportCsvDefinition = {
      HasHeaders: this.hasHeaders,
      TransactionDate_Index: -1,
      TransactionType_Index: -1,
      Description_Index: -1,
      DebitAmount_Index: -1,
      CreditAmount_Index: -1,
      Amount_Index: -1
    };

    for (const i in this.ColumnMapping) {
      if (this.ColumnMapping[i].ColumnFieldMap === 'TransactionDate') {
        importDefinition.TransactionDate_Index = Number(i);
      }
      if (this.ColumnMapping[i].ColumnFieldMap === 'Description') {
        importDefinition.Description_Index = Number(i);
      }
      if (this.ColumnMapping[i].ColumnFieldMap === 'CreditAmount') {
        importDefinition.CreditAmount_Index = Number(i);
      }
      if (this.ColumnMapping[i].ColumnFieldMap === 'DebitAmount') {
        importDefinition.DebitAmount_Index = Number(i);
      }
      if (this.ColumnMapping[i].ColumnFieldMap === 'TransactionType') {
        importDefinition.TransactionType_Index = Number(i);
      }
      if (this.ColumnMapping[i].ColumnFieldMap === 'Amount') {
        importDefinition.Amount_Index = Number(i);
      }
    }

    return importDefinition;
  }
}
