import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/map';

import { TransactionsService } from '../../services/transactions.service';
import { AccountService } from '../../services/account.service';
import { ColorsService } from '../../core/services/colors.service';

import { CashFlow } from '../dashboard.types';

@Component({
  moduleId: module.id,
  selector: 'dashboard-charts',
  templateUrl: './charts.component.html',
})

export class ChartsComponent implements OnInit {

  public isBusy: any;
  public errorMsg: string;

  public lineChartData: Array<any>;
  public pieData: any;
  public Account: any;

  public lineChartLabels: Array<any>;

  public lineChartOptions: any = {
    responsive: true,
    type: 'bar',
    legend: {
      position: 'bottom',
      labels: { fontSize: 9, fontColor: 'rgb(255, 99, 132)' }
    },
  };


  public transactions: CashFlow[];

  public sparklineValues: any = []

  public sparkOptions2 = {
    type: 'line',
    height: 80,
    width: '100%',
    lineWidth: 2,
    lineColor: this.colors.byName('info'),
    spotColor: this.colors.byName('info'),
    minSpotColor: this.colors.byName('info'),
    maxSpotColor: this.colors.byName('info'),
    fillColor: '',
    highlightLineColor: this.colors.byName('info'),
    spotRadius: 3,
    resize: true
  };


  @Input()
  set accountID(accountID: number) {
    this.LoadTransactions(accountID);
    this.LoadAccount(accountID);
  }

  constructor(
    private transactionsService: TransactionsService,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private colors: ColorsService
  ) { }

  public ngOnInit() {


  }

  private LoadAccount(accountID) {
    this.accountService.getSummary(accountID)
      .subscribe(data => {
        this.Account = data;
      }, err => {
        this.errorMsg = err;
      });
  }

  private LoadTransactions(accountID) {
    this.isBusy = this.transactionsService.getCashFlow(accountID)
      .subscribe(data => {
        this.transactions = data;

        this.lineChartData = [
          {
            data: this.transactions.map(x => x.CreditAmount), label: 'Income'
          },
          {
            data: this.transactions.map(x => x.DebitAmount), label: 'Expenses'
          },
          {
            data: this.transactions.map(x => x.Balance), label: 'Balance', type: 'line'
          },
        ];
        this.lineChartLabels = this.transactions.map(x => x.MonthYear);

        this.sparklineValues = this.transactions.map(x => x.Balance)

      }, err => {
        this.errorMsg = err;
      });

    this.transactionsService.getTagExpenses(accountID)
      .subscribe(data => {

        this.pieData = {
          labels: data.map(x => x.Tag).slice(1, 10),
          datasets: [{
            data: data.map(x => x.Amount).slice(1, 10),
          }]
        };



      }, err => {
        this.errorMsg = err;
      });
  }







}
