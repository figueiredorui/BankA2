import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TagService } from '../services/tag.service';

import { TagListComponent } from './tag-list/tag-list.component';
import { TagDialogComponent } from './tag-dialog/tag-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  exports:[
    TagListComponent 
  ],
   entryComponents: [
    TagDialogComponent
  ],
  providers: [TagService],
  declarations: [TagListComponent, TagDialogComponent]
})
export class TagsModule { }
