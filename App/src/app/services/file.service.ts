import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { HttpApiService } from '../core/services/http-api.service';
import { Account } from '../accounts/accounts.types';

@Injectable()
export class FileService extends HttpApiService {

  constructor(private http: Http) { super(); }

  public parseFile(file: any): Observable<any> {
    const headers = new Headers();
    headers.delete('Content-Type');
    const options = new RequestOptions({ headers: headers });

    const url = `${this.baseUrl}/files/parse`;

    const formData: FormData = new FormData();
    formData.append('formFile', file, file.name);

    return this.http.post(url, formData, options)
      .map(response => response.json() as any)
      .catch(super.handleError2);
  }

  public getFiles(accountId: number): Observable<any> {
    const url = `${this.baseUrl}/accounts/${accountId}/files`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public deleteFile(accountId: number, fileId: number): Observable<any> {
    const url = `${this.baseUrl}/accounts/${accountId}/files/${fileId}`;
    return this.http.delete(url)
      .map(response => response.status as any)
      .catch(super.handleError);
  }

}
