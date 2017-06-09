import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/map';

import { ConfirmationDialogComponent } from '../../core/confirmation-dialog/confirmation-dialog.component';
import { DialogService } from "ng2-bootstrap-modal";

import { FileService } from '../../services/file.service';

@Component({
  selector: 'dashboard-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

  @Input()
  set accountID(accountID: number) {
    this.LoadFiles(accountID);
  }

  public errorMsg: string;
  public files: any[];

  constructor(
    private fileService: FileService,
    private dialogService:DialogService
  ) { }

  public ngOnInit() {


  }

  public deleteFile(accountID, fileID) {

    let disposable = this.dialogService.addDialog(ConfirmationDialogComponent, {
      title: 'Files',
      message: 'Would you like to delete this file?'
    })
      .subscribe((isConfirmed) => {
        //We get dialog result
        if (isConfirmed) {
          this.fileService.deleteFile(accountID, fileID)
            .subscribe(data => {

              this.LoadFiles(accountID);
            }, err => {
              this.errorMsg = err;
            });
        }
      });


  }

  private LoadFiles(accountID) {
    this.fileService.getFiles(accountID)
      .subscribe(data => {
        this.files = data;

      }, err => {
        this.errorMsg = err;
      });
  }

}
