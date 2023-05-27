import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LikeResponse } from 'app/modules/models/like-model';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class LikeService {

  baseUrl: string = environment.baseUrl + 'tweets';

  constructor(private _httpClient: HttpClient) { }

  likeTweet(tweetId: string): Observable<LikeResponse> {

    const requestUrl = `${this.baseUrl}/${tweetId}/like`;

    return this._httpClient.post<LikeResponse>(requestUrl, {})

  }
}
