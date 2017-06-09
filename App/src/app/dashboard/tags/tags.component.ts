import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/map';

import { TagService } from '../../services/tag.service';

@Component({
  selector: 'dashboard-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.css']
})
export class TagsComponent implements OnInit {

  @Input()
  set accountID(accountID: number) {
    this.LoadTags(accountID);
  }

  public errorMsg: string;
  public tags: any[];

  constructor(
    private tagService: TagService,
    private route: ActivatedRoute
  ) { }

  public ngOnInit() {


  }

  
  private LoadTags(accountID) {
    this.tagService.getTagsByAccount(accountID)
      .subscribe(data => {
        this.tags = data;

      }, err => {
        this.errorMsg = err;
      });
  }

}
