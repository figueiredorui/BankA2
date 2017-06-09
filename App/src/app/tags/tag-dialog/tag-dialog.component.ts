import { Component, OnInit } from '@angular/core';
import 'rxjs/add/operator/catch';
import { DialogComponent, DialogService } from 'ng2-bootstrap-modal';

import { TagService } from '../../services/tag.service';
import { Tag } from '../tag.types';

export interface TagDialogParam {
  tagID: number;
  transaction: any
}

@Component({
  selector: 'app-tag-dialog',
  templateUrl: './tag-dialog.component.html',
})
export class TagDialogComponent extends DialogComponent<TagDialogParam, Tag> implements TagDialogParam, OnInit {

  public tagID: number;
  public transaction: any
  public errorMsg: string = null;
  public tag: Tag;

  constructor(dialogService: DialogService, private tagService: TagService) {
    super(dialogService);
  }

  public ngOnInit() {
    this.loadTag();
  }

  private loadTag() {
    if (this.tagID > 0) {
      this.tagService.getTag(this.tagID)
        .subscribe(data => this.tag = data,
        err => this.errorMsg = err);
    } else {
      this.tag = {
        TagId: 0,
        Description: '',
        Tag: '',
      };

      if (this.transaction){
        this.tag.Description = this.transaction.Description
      }

    }
  }

  public cancel() {
    this.result = null;
    this.close();
  }

  public saveTag() {
    this.tagService.saveTag(this.tag)
      .subscribe(data => {
        this.tag = data;
        this.result = this.tag;
        this.close();
      }, err => this.errorMsg = err);
  }

  public deleteTag() {
    this.tagService.deleteTag(this.tag.TagId)
      .subscribe(data => {
        
        this.result = this.tag;
        this.close();
      }, err => this.errorMsg = err);
  }
}
