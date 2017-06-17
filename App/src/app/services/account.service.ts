import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { HttpApiService } from '../core/services/http-api.service';
import { Account } from '../accounts/accounts.types';

@Injectable()
export class AccountService extends HttpApiService {

  constructor(private http: Http) { super(); }

public getList(): Observable<any[]> {
    const url = `${this.baseUrl}/accounts`;
    return this.http.get(url)
      .map(response => response.json() as any[])
      .catch(super.handleError);
  }

  public getSummary(id: number): Observable<any[]> {
    const url = `${this.baseUrl}/accounts/${id}/Summary`;
    return this.http.get(url)
      .map(response => response.json() as any[])
      .catch(super.handleError);
  }

  public getAccount(id: number): Observable<any> {
    const url = `${this.baseUrl}/accounts/${id}`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public saveAccount(account: Account): Observable<Account> {
    if (account.AccountId > 0) {
      return this.updateAccount(account);
    } else {
      return this.addAccount(account);
    }
  }

  public deleteAccount(id: number): Observable<any> {
    const url = `${this.baseUrl}/accounts/${id}`;
    return this.http.delete(url)
      .map(response => response.status as any)
      .catch(super.handleError);
  }

  private addAccount(account: Account): Observable<Account> {
    const url = `${this.baseUrl}/accounts`;
    return this.http.post(url, JSON.stringify(account))
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  private updateAccount(account: Account): Observable<Account> {
    const url = `${this.baseUrl}/accounts/${account.AccountId}`;
    return this.http.put(url, JSON.stringify(account))
      .map(response => response.json() as any)
      .catch(super.handleError);
  }
}
