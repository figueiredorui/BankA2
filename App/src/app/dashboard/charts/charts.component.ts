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


 

  // window.chartColors = {
  // 	red: 'rgb(255, 99, 132)',
  // 	orange: 'rgb(255, 159, 64)',
  // 	yellow: 'rgb(255, 205, 86)',
  // 	green: 'rgb(75, 192, 192)',
  // 	blue: 'rgb(54, 162, 235)',
  // 	purple: 'rgb(153, 102, 255)',
  // 	grey: 'rgb(201, 203, 207)'
  // };

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
            stack: 'Stack 0',
            data: this.transactions.map(x => x.CreditAmount), label: 'Income'
          },
          {
            stack: 'Stack 0',
            data: this.transactions.map(x => x.TransferInAmount), label: 'Transfer In',
            backgroundColor:'rgba(255, 99, 132, 0.2)'
          },
          {
            stack: 'Stack 1',
            data: this.transactions.map(x => x.DebitAmount), label: 'Expenses'
          },
          {
            stack: 'Stack 1',
            data: this.transactions.map(x => x.TransferOutAmount), label: 'Transfer Out'
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

    this.transactionsService.getTop10Expenses(accountID)
      .subscribe(data => {

        this.pieData = {
          labels: data.map(x => x.Tag),
          datasets: [{
            data: data.map(x => x.Amount),
          }]
        };



      }, err => {
        this.errorMsg = err;
      });
  }







}
