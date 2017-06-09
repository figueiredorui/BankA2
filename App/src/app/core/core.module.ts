import { NgModule} from '@angular/core';
    import { CommonModule } from "@angular/common";
    import { BrowserModule } from '@angular/platform-browser';
    import { BootstrapModalModule } from 'ng2-bootstrap-modal';
    import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
    
    @NgModule({
      declarations: [
        ConfirmationDialogComponent
      ],
      imports: [
        CommonModule,
        BrowserModule,
        BootstrapModalModule
      ],
      //Don't forget to add the component to entryComponents section
      entryComponents: [
        ConfirmationDialogComponent
      ],
      exports:[ConfirmationDialogComponent]
    })
export class CoreModule { }
