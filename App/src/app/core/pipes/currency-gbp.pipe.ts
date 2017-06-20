import { Pipe } from '@angular/core';

@Pipe({ name: 'currencyGBP' })
export class CurrencyGPB {

  transform(value: any) {
      if (value && !isNaN(value)) {
          return '£' + parseFloat(value).toFixed(2);
      }
      return '£0.00';
  }

}