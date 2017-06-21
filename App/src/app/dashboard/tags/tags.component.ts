import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/map';

import { TransactionsService } from '../../services/transactions.service';
import { ColorsService } from '../../core/services/colors.service';
import { TagSummary } from '../dashboard.types';

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

  public isBusy: any;
  public errorMsg: string;
  public tagExpenses: Array<TagSummary>;

  public sparkOptions1 = {
    type: 'line',
    height: 20,
    width: '50%',
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

  constructor(
    private transactionsService: TransactionsService,
    private colors: ColorsService,
    private route: ActivatedRoute
  ) { }

  public ngOnInit() {


  }


  private LoadTags(accountID) {
    this.isBusy = this.transactionsService.getTagDetails(accountID)
      .subscribe(data => {
        this.tagExpenses = data;

        this.tagExpenses.forEach(element => {
          element.MonthlyAmount = element.Details.map(m => m.Amount);
        });

      }, err => {
        this.errorMsg = err;
      });
  }

}
