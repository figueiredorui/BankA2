import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { HttpApiService } from '../core/services/http-api.service';
import { Tag } from '../tags/tag.types';

@Injectable()
export class TagService extends HttpApiService {

  constructor(private http: Http) { super(); }

public getTag(id: number): Observable<Tag> {
    const url = `${this.baseUrl}/Tags/${id}`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public getTags(): Observable<Tag[]> {
    const url = `${this.baseUrl}/Tags`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public getTagsLookup(): Observable<any>{
    const url = `${this.baseUrl}/tags/lookup`;
    return this.http.get(url)
      .map(response => response.json() as any)
      .catch(super.handleError);
  }

  public saveTag(Tag: Tag): Observable<Tag> {
    if (Tag.TagId > 0) {
      return this.updateTag(Tag);
    } else {
      return this.addTag(Tag);
    }
  }

  public deleteTag(id: number): Observable<Tag> {
    const url = `${this.baseUrl}/Tags/${id}`;
    return this.http.delete(url)
      .map(response => response.status as any)
      .catch(super.handleError);
  }

  private addTag(Tag: Tag): Observable<Tag> {
    const url = `${this.baseUrl}/Tags`;
    return this.http.post(url, JSON.stringify(Tag))
      .map(response => response.json() as any)
      .catch(super.handleError2);
  }

  private updateTag(Tag: Tag): Observable<Tag> {
    const url = `${this.baseUrl}/Tags/${Tag.TagId}`;
    return this.http.put(url, JSON.stringify(Tag))
      .map(response => response.json() as any)
      .catch(super.handleError);
  }
}
