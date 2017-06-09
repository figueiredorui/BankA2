import { Component, OnInit, ViewContainerRef } from '@angular/core';
import 'rxjs/add/operator/map';
import { DialogService } from 'ng2-bootstrap-modal';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

import { TagService } from '../../services/tag.service';
import { Tag } from '../tag.types';

import { TagDialogComponent } from '../tag-dialog/tag-dialog.component'

@Component({
  selector: 'app-tag-list',
  templateUrl: './tag-list.component.html',
})
export class TagListComponent implements OnInit {

  public errorMsg: string;
  public tags: Tag[];
  public tagID: number;


  constructor(
    private tagService: TagService,
    private dialogService: DialogService,
    private toastr: ToastsManager,
    private vcr: ViewContainerRef
  ) { 
    this.toastr.setRootViewContainerRef(vcr);
  }

  public ngOnInit() {
    this.LoadTags();

  }

  public addTag() {
    const disposable = this.dialogService.addDialog(TagDialogComponent, { tagID: 0 }, { backdropColor: 'rgba(0, 0, 0, 0.4)' })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.LoadTags();
          this.toastr.success('Deleted.', 'Success!');
        }
      });
  }

  public editTag(tagID) {
    const disposable = this.dialogService.addDialog(TagDialogComponent, { tagID: tagID }, { backdropColor: 'rgba(0, 0, 0, 0.4)' })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.LoadTags();
          this.toastr.success('Deleted.', 'Success!');
        }
      });
  }

  public DeleteTag(tagID) {
    this.tagService.deleteTag(tagID)
      .subscribe(data => {
        this.LoadTags();
        this.toastr.success('Deleted.', 'Success!');
      }, err => {
        this.errorMsg = err;
      });
  }

  private LoadTags() {
    this.tagService.getTags()
      .subscribe(data => {
        this.tags = data;

      }, err => {
        this.errorMsg = err;
      });
  }



}
