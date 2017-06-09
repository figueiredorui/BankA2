import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/map';

import { TransactionsService } from '../../services/transactions.service';

import { CashFlow } from '../dashboard.types';

@Component({
  moduleId: module.id,
  selector: 'dashboard-charts',
  templateUrl: './charts.component.html',
})

export class ChartsComponent implements OnInit {

  public errorMsg: string;
  // 68B3C8
  // EB5E28
  public lineChartColors1: Array<any> = [
    { // #68B3C8
      backgroundColor: '#95CAD8',
      borderColor: '#68B3C8',
    },
    { // #68B3C8
      backgroundColor: '#E47A3D',
      borderColor: '#EB5E28',
    }
  ];
  // lineChart
  public lineChartData: Array<any>;
  public lineChartLabels: Array<any>;

  public lineChartOptions: any = { responsive: true };

  public lineChartLegend: boolean = true;
  public lineChartType: string = 'line';

  public transactions: CashFlow[];

  @Input()
  set accountID(accountID: number) {
    this.LoadTransactions(accountID);
  }

  constructor(
    private transactionsService: TransactionsService,
    private route: ActivatedRoute
  ) { }

  public ngOnInit() {

    
  }

  private LoadTransactions(accountID) {
    this.transactionsService.getCashFlow(accountID)
      .subscribe(data => {
        this.transactions = data;

        this.lineChartData = [
          {
            data: this.transactions.map(x => x.CreditAmount)
            , label: 'Income'
          },
          {
            data: this.transactions.map(x => x.DebitAmount), label: 'Expenses'
          },
        ];
        this.lineChartLabels = this.transactions.map(x => x.Month);

      }, err => {
        this.errorMsg = err;
      });
  }







}
