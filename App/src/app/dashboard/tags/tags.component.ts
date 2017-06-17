import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/map';

import { TransactionsService } from '../../services/transactions.service';
import { TagExpenses } from '../dashboard.types';

@Component({
  selector: 'dashboard-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.scss']
})
export class TagsComponent implements OnInit {

  @Input()
  set accountID(accountID: number) {
    this.LoadTags(accountID);
  }

  public errorMsg: string;
  public tagExpenses: Array<TagExpenses>;

  public tagExpensesDetails: any = [1,3,4,7,5,9,4,4,7,5,9,6,4]
  public sparkOptions1 = {
        barColor: '#23b7e5',
        height: 30,
        barWidth: '5',
        barSpacing: '2'
    };

  constructor(
    private transactionsService: TransactionsService,
    private route: ActivatedRoute
  ) { }

  public ngOnInit() {


  }

  
  private LoadTags(accountID) {
    this.transactionsService.getTagExpenses(accountID)
      .subscribe(data => {
        this.tagExpenses = data;

        this.tagExpenses.forEach(element => {
          element.MonthlyAmount = element.Details.map(m=>m.Amount);
        });

      }, err => {
        this.errorMsg = err;
      });
  }

}
