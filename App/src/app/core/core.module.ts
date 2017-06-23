import { NgModule} from '@angular/core';
    import { CommonModule } from "@angular/common";
    import { BrowserModule } from '@angular/platform-browser';
    import { BootstrapModalModule } from 'ng2-bootstrap-modal';

    import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
    import { SparklineDirective } from './directives/sparkline.directive';
    import { BaseChartDirective } from './directives/charts.directive';
    
    import { CurrencyGPB } from './pipes/currency-gbp.pipe';
    import { HttpApiService } from './services/http-api.service';
    
    @NgModule({
      declarations: [
        ConfirmationDialogComponent,
        SparklineDirective,
        BaseChartDirective,
        CurrencyGPB 
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
      exports:[
        ConfirmationDialogComponent,
        SparklineDirective,
        BaseChartDirective,
        CurrencyGPB
      ]
    })
export class CoreModule { }
