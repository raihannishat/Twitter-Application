import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { TweetViewModel } from '../../../modules/models/tweet-model';



@Injectable({
  providedIn: 'root'
})
export class SearchService {

  hashTweet: TweetViewModel[];
  searchUrl = environment.baseUrl + 'search';
  baseUrl = environment.baseUrl + 'tweets/';

  constructor(private httpClient: HttpClient,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  getHashPost(value: string, pageNumber: number = 1) {
    const _value = (value.includes('#') ? value.slice(1) : value);
    const hashPost = `${this.baseUrl}hashtag-tweets?keyword=${_value}&page=${pageNumber}`;
    this.httpClient.get(hashPost)
      .subscribe((hashPostResult: TweetViewModel[]) => {
        if (pageNumber > 1) {
          hashPostResult.map((post) => {
            this.hashTweet.push(post);
          });
        }
        else {
          this.hashTweet = hashPostResult;
        }
        this.router.navigate(['/post'], { queryParams: { search: _value } });

      });
  }

  getUsersByName(name: string): Observable<any> {
    const requestUrl = `${this.searchUrl}/users?name=${name}`;
    return this.httpClient.get(requestUrl);
  }

  getUsersByHashtag(hashtag: string): Observable<any> {
    const _value = hashtag.slice(1);
    const hashValue = `${this.searchUrl}?hashtag=${_value}`;
    return this.httpClient.get(hashValue);
  }

}
