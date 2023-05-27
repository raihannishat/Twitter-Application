import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RetweetResponse } from 'app/modules/models/retweet-model';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class RetweetService {

  baseUrl: string = environment.baseUrl + 'tweets';

  constructor(private _httpClient: HttpClient) { }

  retweet(tweetId: string): Observable<RetweetResponse> {

    const requestUrl = `${this.baseUrl}/${tweetId}/retweet`;

    return this._httpClient.post<RetweetResponse>(requestUrl, {})

  }
}
