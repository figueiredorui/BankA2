import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

import { TransactionsService } from '../../services/transactions.service';

export interface TagsDetailsParam {
  Tag: string;
}

@Component({
  selector: 'tags-details',
  templateUrl: './tags-details.component.html',
})
export class TagsDetailsComponent extends DialogComponent<TagsDetailsParam, boolean> implements TagsDetailsParam, OnInit {

  Tag: string;

  constructor(
    dialogService: DialogService,
    private transactionService: TransactionsService,
  ) {
    super(dialogService);
  }

  public ngOnInit() {

  }

}
