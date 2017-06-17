import { Injectable } from '@angular/core';
import { Http, Response, BaseRequestOptions, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../environments/environment';


export class HttpApiService {

  protected baseUrl = environment.apiUrl;

  constructor() {

  }

  protected handleError(error: any): Promise<any> {
    console.log('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

  protected handleError2(error: Response | any) {
    // In a real world app, you might use a remote logging infrastructure
    let errMsg: string;
    if (error instanceof Response) {
      const body = error['_body'] || '';
      // const err = body.error || JSON.stringify(body);
      errMsg = `${error.status} - ${error.statusText || ''} - ${body}`;
    } else {
      errMsg = error.message ? error.message : error.toString();
    }
    console.error(errMsg);
    return Observable.throw(errMsg);
  }

}


@Injectable()
export class DefaultRequestOptions extends BaseRequestOptions {

  constructor() {
    super();

    // Set the default 'Content-Type' header
    this.headers.set('Content-Type', 'application/json');
  }
}

export const RequestOptionsProvider = { provide: RequestOptions, useClass: DefaultRequestOptions };
