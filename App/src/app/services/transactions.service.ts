import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { HttpApiService } from '../core/services/http-api.service';
import { TransactionResult, CashFlow, TagSummary,TagExpense, TransactionSearch, Transaction, ImportCsvDefinition, BalanceView } from '../dashboard/dashboard.types';

@Injectable()
export class TransactionsService extends HttpApiService {

  constructor(private http: Http) { super(); }

  public getTransactions(id: number, search: TransactionSearch): Observable<TransactionResult> {
    const url = `${this.baseUrl}/accounts/${id}/transactions?page=${search.Page}&query=${search.Query}`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public getCashFlow(id: number): Observable<CashFlow[]> {
    const url = `${this.baseUrl}/accounts/${id}/cashflow`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public getBalance(id: number): Observable<BalanceView[]> {
    const url = `${this.baseUrl}/accounts/${id}/balance`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public getTagDetails(id: number): Observable<TagSummary[]> {
    const url = `${this.baseUrl}/accounts/${id}/TagDetails`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public getTop10Expenses(id: number): Observable<TagExpense[]> {
    const url = `${this.baseUrl}/accounts/${id}/Top10Expenses`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public updateTag(transaction: Transaction): Observable<Transaction> {
    const url = `${this.baseUrl}/transactions/${transaction.TransactionId}/tag`;
    return this.http.put(url, JSON.stringify(transaction.Tag))
      .map(response => response as any)
      .catch(super.handleError2);
  }

  public markAsTransfer(transaction: Transaction): Observable<Transaction> {
    const url = `${this.baseUrl}/transactions/${transaction.TransactionId}/markastransfer`;
    return this.http.put(url, JSON.stringify(transaction.IsTransfer))
      .map(response => response as any)
      .catch(super.handleError2);
  }

  public import(accountID: number, importDefinition: ImportCsvDefinition, file: any): Observable<any> {
    const headers = new Headers();
    headers.delete('Content-Type');
    const options = new RequestOptions({ headers: headers });

    const url = `${this.baseUrl}/Accounts/${accountID}/transactions/import`;

    const formData: FormData = new FormData();
    formData.append('formFile', file, file.name);

    Object.keys(importDefinition).forEach(key => formData.append(key, importDefinition[key]));

    return this.http.post(url, formData, options)
      .map(response => response as any)
      .catch(super.handleError2);
  }
}
